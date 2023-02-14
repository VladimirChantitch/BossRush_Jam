using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;
using player;
using System;

public class PlayerMovement : MonoBehaviour
{
    private PlayerManager playerManager;

    private Rigidbody2D rb;
    private Controls controls;

    private Animator animator;
    [SerializeField] TrailRenderer trailRenderer;

    public GameObject camera;
    private CameraJuice camJuice;

    [Header("Movement")]
    public float mvtVelocity; //need to be relocated in Player Stat Struct
    private Vector2 inputVector;
    private bool isMoving = false;


    [Header("Ground")]
    public Collider2D groundCheckCollider;
    public ContactFilter2D groundFilter;
    private bool isGrounded = false;


    [Header("Jump")]
    public float jumpVelocity; //need to be relocated in Player Stat Struct
    private bool airJumpingReady = true;
    public float airjumpVelocity; //need to be relocated in Player Stat Struct
    public Collider2D defaultCollider2D;
    public Collider defaultCollider3D;
    public Collider2D jumpCollider2D;
    public Collider jumpCollider3D;
    public Vector2 defaultGroundCheckPos;
    public Vector2 jumpgroundCheckPos;


    [Header("Dash")]
    public bool canDash;
    public bool isDashing;
    public float dashVelocity;
    public float dashingTime;
    private Vector2 dashingDir;
    private int maxDash;
    private int dashPool;
    private bool dashCooldown = false;
    public UnityEvent StartDash;
    public UnityEvent EndDash;

    [Header("Flip")]
    private bool isFlipped = false;

    [Header("Attack")]
    public UnityEvent<AttackType> attackPerformed = new UnityEvent<AttackType>();
    public UnityEvent onAttack2Stopped = new UnityEvent();
    public RythmBonus rythmBonus;
    public TrailRenderer guitareTrail;
    public Transform aoe_gfx;
    bool aoeTrigger = false;
    float timeElapsed;
    float lerpDuration = 1;


    private void Awake()
    {
        playerManager = GetComponentInChildren<PlayerManager>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        camJuice = GetComponent<CameraJuice>();

        controls = new Controls();
        controls.Player.Enable();
        controls.Player.Jump.performed += Jump_performed;
        controls.Player.Movement.performed += Movement_performed;
        controls.Player.Movement.canceled += Movement_canceled;
        controls.Player.Dash.performed += Dash_performed;
        controls.Player.MousePosition.performed += MousePosition_performed;
        controls.Player.Attack1.performed += Attack1_performed;
        controls.Player.Attack1.canceled += Attack1_canceled;
        controls.Player.Attack2.performed += Attack2_performed;
        controls.Player.Attack2.canceled += Attack2_canceled;

        maxDash = dashPool = (int)playerManager.GetStat(Boss.stats.StatsType.dash).MaxValue;

        playerManager.playerUIManager.SetDash(dashPool);

    }

    public void Dispose()
    {
        controls.Player.Jump.performed -= Jump_performed;
        controls.Player.Movement.performed -= Movement_performed;
        controls.Player.Movement.canceled -= Movement_canceled;
        controls.Player.Dash.performed -= Dash_performed;
        controls.Player.MousePosition.performed -= MousePosition_performed;
        controls.Player.Attack1.performed -= Attack1_performed;
        controls.Player.Attack1.canceled -= Attack1_canceled;
        controls.Player.Attack2.performed -= Attack2_performed;
        controls.Player.Attack2.canceled -= Attack2_canceled;

        controls.Dispose();
    }

    private void Update()
    {
        GroundCheck();
        

        if (dashPool < maxDash && !dashCooldown) //Pour l'instant ca recharge tt le temps. On peut faire en sorte que ca recharge que quand tu es static
        {
            StartCoroutine(DashCooldown());
        }

        if(playerManager.isAtk2)
        {
            aoeTrigger = true;
            EnbiggenAOE();
        }
        else if(!playerManager.isAtk2 && aoeTrigger)
        {
            aoeTrigger = false;
            timeElapsed = 0;
            aoe_gfx.localScale = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if(isDashing)
        {
            rb.velocity = dashingDir * dashVelocity;
            return;
        }

        if(isMoving)
        {
            inputVector = controls.Player.Movement.ReadValue<Vector2>();

            if (inputVector.x != 0)
            {


                animator.SetBool("isMoving", true);
                rb.velocity = new Vector2(inputVector.x * mvtVelocity, rb.velocity.y);
            }
        }
    }

    private void LateUpdate()
    {
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isInAir", false);
            SetJumpColliders(false);
        }
        
        if(!isGrounded && !animator.GetBool("isJumping"))
        {
            animator.SetBool("isInAir", true);
        }

        if(rb.velocity.x < 5 && rb.velocity.x > -5 && rb.velocity.x != 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void Attack1_performed(InputAction.CallbackContext context)
    {
        camJuice.AtkZoom();
        animator.Play("Attack");
        guitareTrail.emitting = true;
        rythmBonus.BaseAttack(0);
        attackPerformed?.Invoke(AttackType.normal);
    }

    private void Attack1_canceled(InputAction.CallbackContext obj)
    {
        camJuice.Default();
        guitareTrail.emitting = false;
    }

    private void Attack2_performed(InputAction.CallbackContext context)
    {
        camJuice.AtkZoom();
        timeElapsed = 0;
        playerManager.OpenAttackCollider_Big();
        animator.Play("RiffAttack");
        rythmBonus.BaseAttack(1);
        attackPerformed?.Invoke(AttackType.big);
    }

    private void Attack2_canceled(InputAction.CallbackContext obj)
    {
        camJuice.Default();
        onAttack2Stopped?.Invoke();
    }

    private void EnbiggenAOE()
    {
        aoe_gfx.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 10f, timeElapsed / lerpDuration);
        timeElapsed += Time.deltaTime;
    }

    private void GroundCheck()
    {
        isGrounded = groundCheckCollider.IsTouching(groundFilter);
        if (!airJumpingReady)
            airJumpingReady = groundCheckCollider.IsTouching(groundFilter);
    }

    private void Movement_performed(InputAction.CallbackContext context)
    {
        if(!isDashing)
        {
            camJuice.MovementDezoom();
            isMoving = true;
        }
    }

    private void Movement_canceled(InputAction.CallbackContext context)
    {
        camJuice.Default();
        rb.velocity = new Vector2(0f, rb.velocity.y);
        inputVector = Vector2.zero;
        animator.SetBool("isMoving", false);
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        if(!isDashing)
        {
            if (isGrounded) //Normal Jump
            {
                groundCheckCollider.enabled = false;
                StartCoroutine(ActivateGroundCheckCollider());
                rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
                animator.SetBool("isJumping", true);
                SetJumpColliders(true);
            }
            else if (!isGrounded && airJumpingReady)
            {
                airJumpingReady = false;
                rb.velocity = new Vector2(rb.velocity.x, airjumpVelocity);

                animator.SetBool("isJumping", true);
                SetJumpColliders(true);
            }
        }
    }

    private IEnumerator ActivateGroundCheckCollider()
    {
        yield return new WaitForSeconds(0.1f);
        groundCheckCollider.enabled = true;
    }

    private void SetJumpColliders(bool flag)
    {
        defaultCollider2D.enabled = defaultCollider3D.enabled = !flag;
        jumpCollider2D.enabled = jumpCollider3D.enabled = flag;
        if (flag)
            groundCheckCollider.offset = jumpgroundCheckPos;
        else
            groundCheckCollider.offset = defaultGroundCheckPos;
    }

    private void Dash_performed(InputAction.CallbackContext context) //Hold Shift
    {
        if (dashPool > 0)
        {
            StartDash?.Invoke();
            camJuice.DashZoom();
            trailRenderer.emitting = true;
            isDashing = true;   
            dashPool--;
            playerManager.playerUIManager.SetDash(dashPool);
            dashingDir = new Vector2(inputVector.x, 0);
            if(inputVector.x == 0)
            {
                dashingDir = new Vector2(transform.localScale.x, 0);
            }
            PlayDashAnimation(isFlipped, dashingDir.x, true);
            StartCoroutine(StopDashing());
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        EndDash?.Invoke();
        PlayDashAnimation(isFlipped, dashingDir.x, false);
        trailRenderer.emitting = false;
        if (animator.GetBool("isMoving"))
        {
            rb.velocity = new Vector2(inputVector.x * mvtVelocity, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        isDashing = false;
        camJuice.Default();
    }

    private void PlayDashAnimation(bool isFlipped, float dir, bool flag)
    {
        if(!isFlipped && dir >= 0)
        {
            animator.SetBool("isForwardDash", flag);
        }
        else if(!isFlipped && dir < 0)
        {
            animator.SetBool("isBackDash", flag);
        }
        else if(isFlipped && dir >= 0)
        {
            animator.SetBool("isBackDash", flag);
        }
        else if (isFlipped && dir < 0)
        {
            animator.SetBool("isForwardDash", flag);
        }
    }

    private IEnumerator DashCooldown()
    {
        dashCooldown = true;
        yield return new WaitForSeconds(1.75f);
        dashPool++;
        playerManager.playerUIManager.SetDash(dashPool);
        dashCooldown = false;
    }

    private void MousePosition_performed(InputAction.CallbackContext context)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.farClipPlane * .5f;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);

        if(worldPoint.x - transform.position.x >= 0 && isFlipped || worldPoint.x - transform.position.x <= 0 && !isFlipped)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        isFlipped = !isFlipped;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
