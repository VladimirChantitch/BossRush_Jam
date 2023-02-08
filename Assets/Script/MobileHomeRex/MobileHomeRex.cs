using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileHomeRex : BossCharacter
{
    public enum MobileHomeState
    {
        Idle,
        Appearing,
        AttackingRun,
        RunningBack,
        AttackingBite,
        AttackingCannon,
        AttackingTurtle_Low,
        AttackingTurtle_High,
        Dying,
        PhaseTransition,
    }

    private Camera cam;
    private Rigidbody2D rb;
    private Animator animator;

    public Transform RightBase;
    public Transform LeftBase;

    private bool isFlipped = false;
    
    public float speedFront;
    public float speedBack;

    [Header("State")]
    [SerializeField] private MobileHomeState state;
    public bool isAttacking;
    public bool isVulnerable;
    public bool isDamageable;
    public bool isArriving;
    public bool isAwaiting;
    public bool isDying;

    public GameObject[] shootingPoints;

    public MHR_DamageCollider damageCollider;

    [Header("Animations")]
    private string _Idle = "Idle";
    private string _Appearing = "Appearing";
    private string _Running = "Running";
    private string _RunningBack = "RunningBack";
    private string _BiteAttack = "BiteAttack";
    private string _CannonAttack = "CannonAttack";
    private string _BackAttack_Low = "BackAttack_Low";
    private string _BackAttack_High = "BackAttack_High";
    private string _Death = "Death";

    private float timePlayerInAir = 7.5f;
    private bool coolDownDef = false;
    private float airDefenseCoolDown = 2f;
    protected float m_Timer;


    // Start is called before the first frame update
    void Start()
    {
        base.Init();

        cam = Camera.main;
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        damageCollider.TakeDamageEvent.AddListener(amount => {
            AddDamage(amount);
            onBossHit?.Invoke();
        });


        Flip();

        initBoss();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDying)
        {
            if (cam.transform.position.x - transform.position.x >= 0 && !isFlipped || cam.transform.position.x - transform.position.x <= 0 && isFlipped)
            {
                Flip();
            }

            if (cam.transform.position.y - transform.position.y >= 5 && AirDefense())
            {
                DefensePosition();
            }

            HandleState();
        }

        if (Phase() == 0)
        {
            Debug.Log("Dead");
            state = MobileHomeState.Dying;
            return;
        }
    }

    private void HandleState()
    {
        switch (state)
        {
            case MobileHomeState.Idle:
                Awaiting();
                break;
            case MobileHomeState.Appearing:
                Appearing();
                break;
            case MobileHomeState.AttackingRun:
                isAwaiting = false;
                AttackingRun();
                break;
            case MobileHomeState.RunningBack:
                isAwaiting = false;
                RunningBack();
                break;
            case MobileHomeState.AttackingBite:
                isAwaiting = false;
                AttackingBite();
                break;
            case MobileHomeState.AttackingCannon:
                isAwaiting = false;
                AttackingCannon();
                break;
            case MobileHomeState.AttackingTurtle_Low:
                isAwaiting = false;
                AttackingTurtle_Low();
                break;
            case MobileHomeState.AttackingTurtle_High:
                isAwaiting = false;
                AttackingTurtle_High();
                break;
            case MobileHomeState.Dying:
                Dying();
                break;
        }
    }

    public void initBoss()
    {
        state = MobileHomeState.Appearing;
    }

    private void Appearing()
    {
        if (isArriving == false)
        {
            isArriving = true;
            PlayTargetAnimation(_Appearing, 1f);
        }
    }

    public void EnterState(MobileHomeState newState)
    {
        state = newState;
    }

    private void Awaiting()
    {
        PlayTargetAnimation(_Idle, 1f);
        foreach(GameObject go in shootingPoints)
        {
            if(go.activeSelf)
            {
                go.SetActive(false);
            }
        }
    }

    private void PrepareToRun()
    {
        if(IsReadyToRun())
            state = MobileHomeState.AttackingRun;
        else
            state = MobileHomeState.RunningBack;
    }

    private void AttackingRun()
    {
        if (!isFlipped && IsReadyToRun())
        {
            PlayTargetAnimation(_Running, 1f);
            rb.velocity = Vector3.left * speedFront;
        }
        else if (isFlipped && IsReadyToRun())
        {
            PlayTargetAnimation(_Running, 1f);
            rb.velocity = Vector3.right * speedFront;
        }
    }

    private void RunningBack()
    {
        if (!isFlipped && transform.position.x - RightBase.position.x <= 0)
        {
            PlayTargetAnimation(_RunningBack, 1f);
            rb.velocity = Vector3.right * speedBack;
        }
        else if (isFlipped && transform.position.x - LeftBase.position.x >= 0)
        {
            PlayTargetAnimation(_RunningBack, 1f);
            rb.velocity = Vector3.left * speedBack;
        }
    }

    private bool IsReadyToRun()
    {
        if (!isFlipped && transform.position.x - RightBase.position.x <= 0)
        {
            return false;
        }
        else if (isFlipped && transform.position.x - LeftBase.position.x >= 0)
        {
            return false;
        }
        return true;
    }

    private void AttackingBite()
    {
        PlayTargetAnimation(_BiteAttack, 1f);
    }

    private void AttackingCannon()
    {
        PlayTargetAnimation(_CannonAttack, 1f);
        shootingPoints[0].SetActive(true);
        shootingPoints[1].SetActive(true);
    }

    private void AttackingTurtle_Low()
    {
        PlayTargetAnimation(_BackAttack_Low, 1f);
        shootingPoints[2].SetActive(true);
    }

    private void AttackingTurtle_High()
    {
        PlayTargetAnimation(_BackAttack_High, 1f);
        shootingPoints[3].SetActive(true);
    }

    private void StopShooting(int index)
    {
        shootingPoints[index].SetActive(false);
    }

    private void StopAllShooting()
    {
        for (int i = 0; i < shootingPoints.Length; i++)
        {
            shootingPoints[i].SetActive(false);
        }
    }

    private void Dying()
    {
        isDying = true;
        onBossDying?.Invoke();
        PlayTargetAnimation(_Death, 1f);
        bossLoot.Loot(inventory.GetItems());
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        isFlipped = !isFlipped;
        localScale.x *= -1f;
        transform.localScale = localScale;

        if (shootingPoints != null)
        {
            for (int i = 0; i < shootingPoints.Length; i++)
            {
                shootingPoints[i].GetComponent<BulletSpawner>().dir = (int)transform.localScale.x;
            }
        }
    }

    public int Phase()
    {
        float currentHealth = GetStat(Boss.stats.StatsType.health).Value;
        float maxHealth = GetStat(Boss.stats.StatsType.health).MaxValue;

        if (currentHealth > maxHealth * 0.5f)
            return 1;
        else if (currentHealth <= maxHealth * 0.5f && currentHealth > maxHealth * 0.25f)
            return 2;
        else if (currentHealth < maxHealth * 0.25f && currentHealth > 0)
            return 3;
        else if (currentHealth <= 0)
            return 0;
        return -1;
    }

    private void ChooseAttack()
    {
        float rnd = UnityEngine.Random.Range(0.0f, 1.0f);
        switch(Phase())
        {
            case 1:
                if (rnd < 0.5f)
                    PrepareToRun();
                else if (rnd >= 0.5f && rnd < 0.85f)
                    state = MobileHomeState.AttackingCannon;
                else
                    state = MobileHomeState.AttackingTurtle_Low;
                break;

            case 2:
                if (rnd < 0.25f)
                    PrepareToRun();
                else if (rnd >= 0.25f && rnd < 0.75f)
                    state = MobileHomeState.AttackingCannon;
                else
                    state = MobileHomeState.AttackingTurtle_Low;
                break;

            case 3:
                if (rnd <= 0.5f)
                    state = MobileHomeState.AttackingCannon;
                else
                    state = MobileHomeState.AttackingTurtle_High;
                break;
            case 0:
                isDying = true;
                break;
        }

    }

    private bool AirDefense()
    {
            m_Timer += Time.deltaTime;
        if(!coolDownDef)
        {
            if (m_Timer > timePlayerInAir)
            {
                m_Timer = 0;
                coolDownDef = true;
                return true;
            }
        }
        else
        {
            if (m_Timer > airDefenseCoolDown)
            {
                m_Timer = 0;
                coolDownDef = false;
                return false;
            }
        }
        return false;
    }

    private void DefensePosition()
    {
        if(Phase() == 3)
        {
            EnterState(MobileHomeState.AttackingTurtle_High);
        }
        else
        {
            EnterState(MobileHomeState.AttackingTurtle_Low);
        }
    }

    public void PlayTargetAnimation(string animationName, float fadeTime)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.CrossFade(animationName, fadeTime);
        animator.Play(animationName);
    }
}
