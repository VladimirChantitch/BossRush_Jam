using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Controls controls;

    private CameraJuice camJuice;

    [Header("Movement")]
    public float mvtVelocity; //need to be relocated in Player Stat Struct
    private Vector2 inputVector;

    [Header("Deceleration")]
    public float decelerationSmooth;
    public float decelerationDeadZone;
    private bool isDecelerating = false;
    private float decelerationSpeed;


    [Header("Ground")]
    public Collider2D groundCheckCollider;
    public ContactFilter2D groundFilter;
    private bool isGrounded = false;


    [Header("Jump")]
    public float jumpVelocity; //need to be relocated in Player Stat Struct
    private bool airJumpingReady = true;
    public float airjumpVelocity; //need to be relocated in Player Stat Struct
    public float airjumpVelocityAfterDash; //need to be relocated in Player Stat Struct

    [Header("Dash")]
    public bool isDashing;
    public float dashVelocity;
    private int dashPool = 3;
    private bool dashCooldown = false;

    /*Temp Canvas*/
    public TextMeshProUGUI dashTMP;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        camJuice = GetComponent<CameraJuice>();

        controls = new Controls();
        controls.Player.Enable();
        controls.Player.Jump.performed += Jump_performed;
        controls.Player.Movement.performed += Movement_performed;
        controls.Player.Movement.canceled += Movement_canceled;
        controls.Player.FrontDash.performed += FrontDash_performed;
        controls.Player.BackDash.performed += BackDash_performed;
        controls.Player.FrontDash.performed += FrontDash_canceled;
        controls.Player.BackDash.performed += BackDash_canceled;

    }

    private void Update()
    {
        isGrounded = groundCheckCollider.IsTouching(groundFilter);
        if (!airJumpingReady)
            airJumpingReady = groundCheckCollider.IsTouching(groundFilter);

        if (isDecelerating)
            decelerationSpeed = Mathf.Lerp(rb.velocity.x, 0, Time.deltaTime * decelerationSmooth);
        if (decelerationSpeed <= decelerationDeadZone && decelerationSpeed >= -decelerationDeadZone)
            decelerationSpeed = 0;

        if(dashPool < 3 && !dashCooldown) //Pour l'instant ca recharge tt le temps. On peut faire en sorte que ca recharge que quand tu es static
        {
            StartCoroutine(DashCooldown());
        }

        /*Temp Canvas*/
        dashTMP.text = dashPool.ToString();
    }

    private void FixedUpdate()
    {
        if (isDecelerating)
            rb.velocity = (new Vector2(decelerationSpeed, rb.velocity.y));
    }

    private void Movement_performed(InputAction.CallbackContext context)
    {
        camJuice.MovementDezoom();
        isDecelerating = false;
        decelerationSpeed = 0;

        inputVector = controls.Player.Movement.ReadValue<Vector2>();
        if (inputVector.x != 0)
            rb.velocity = (new Vector2(inputVector.x * mvtVelocity, rb.velocity.y));
    }

    private void Movement_canceled(InputAction.CallbackContext context)
    {
        isDecelerating = true;
        camJuice.Default();
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        if (isGrounded) //Normal Jump
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        else if (!isGrounded && airJumpingReady)
        {
            airJumpingReady = false;
            if(isDashing)
                rb.AddForce(Vector2.up * airjumpVelocityAfterDash, ForceMode2D.Impulse);
            else
                rb.AddForce(Vector2.up * airjumpVelocity, ForceMode2D.Impulse);
        }
    }

    private void FrontDash_performed(InputAction.CallbackContext context)
    {
        if(dashPool > 0)
        {
            dashPool--;
            camJuice.DashZoom();
            isDashing = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.AddForce(Vector2.right * dashVelocity, ForceMode2D.Impulse);
        }
    }

    private void BackDash_performed(InputAction.CallbackContext context)
    {
        if (dashPool > 0)
        {
            dashPool--;
            camJuice.DashZoom();
            isDashing = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.AddForce(Vector2.left * dashVelocity, ForceMode2D.Impulse);
        }
    }

    private void FrontDash_canceled(InputAction.CallbackContext context)
    {
        StartCoroutine(StopDash());
    }

    private void BackDash_canceled(InputAction.CallbackContext context)
    {
        StartCoroutine(StopDash());
    }

    private IEnumerator StopDash()
    {      
        yield return new WaitForSeconds(0.1f);
        rb.gravityScale = 4f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        isDashing = false;
        yield return new WaitForSeconds(0.2f);
        camJuice.Default();
    }

    private IEnumerator DashCooldown()
    {
        dashCooldown = true;
        yield return new WaitForSeconds(1.75f);
        dashPool++;
        dashCooldown = false;
    }
}
