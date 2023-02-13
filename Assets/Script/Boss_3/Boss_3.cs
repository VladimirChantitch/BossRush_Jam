using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_3 : BossCharacter
{
    public enum Boss_3State
    {
        Idle,
        Appearing,
        Attacking,
        Hiding,
        Spawning,
        Dying,
        PhaseTransition,
    }

    private Animator animator;
    [SerializeField] Transform mainTransform;

    [Header("State")]
    [SerializeField] private Boss_3State state;
    public bool isAttacking;
    public bool isArriving;
    public bool isAwaiting;
    public bool isDying;

    public GameObject[] shootingPoints;

    public MHR_DamageCollider damageCollider;

    [Header("Animations")]
    private string _Idle = "Idle";
    private string _Appearing = "Appearing";
    private string _Attack = "Attack";
    private string _Death = "Death";

    [SerializeField] private bool isMoving = true;
    [SerializeField] float radius;
    float timeElapsed;
    [SerializeField] float lerpDuration = 3;
    private Vector2 originPos;
    private Vector2 targetPos;

    private int moveCounter;
    private int moveMax = 5;

    private int currentBase = 0;
    public Transform[] bossBases;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();

        animator = GetComponent<Animator>();

        damageCollider.TakeDamageEvent.AddListener(amount => {
            AddDamage(amount);
            onBossHit?.Invoke();
        });

        damageCollider.OpenCollider();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            if (timeElapsed < lerpDuration)
            {
                mainTransform.localPosition = Vector2.Lerp(originPos, targetPos, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                Move();
            }
        }

        if (Phase() == 0)
        {
            Dying();
            return;
        }
    }

    private void Move()
    {
        if(moveCounter < moveMax)
        {
            isMoving = true;
            moveCounter++;
            timeElapsed = 0f;
            originPos = new Vector2(mainTransform.localPosition.x, mainTransform.localPosition.y);
            targetPos = UnityEngine.Random.insideUnitCircle * radius + new Vector2(mainTransform.localPosition.x, mainTransform.localPosition.y);
        }
        else
        {
            animator.Play("Despawn");
            moveCounter = 0;
            Spawn();
        }
    }

    private void Spawn()
    {
        int rnd = UnityEngine.Random.Range(0, bossBases.Length);
        if(rnd == currentBase)
        {
            Spawn();
        }
        else
        {
            currentBase = rnd;
            mainTransform.position = bossBases[rnd].position;
            animator.Play("Spawn");
        }
    }

    public int Phase()
    {
        float currentHealth = GetStat(Boss.stats.StatsType.health).Value;
        float maxHealth = GetStat(Boss.stats.StatsType.health).MaxValue;

        if (currentHealth > maxHealth * 0.5f)
        {
            moveMax = 6;
            radius = 5;
            return 1;
        }
        else if (currentHealth <= maxHealth * 0.5f && currentHealth > maxHealth * 0.25f)
        {
            moveMax = 3;
            radius = 10;
            return 2;
        }
        else if (currentHealth < maxHealth * 0.25f && currentHealth > 0)
        {
            moveMax = 2;
            radius = 2;
            return 3;
        }
        else if (currentHealth <= 0)
        {
            return 0;
        }

        return -1;
    }

    private void ChooseAttack()
    {
        float rnd = UnityEngine.Random.Range(0.0f, 1.0f);
        switch (Phase())
        {
            case 1:
                if (rnd < 0.5f)
                {
                    StartCoroutine(Shooting(2, 2.75f));
                }
                else if (rnd >= 0.5f && rnd < 0.85f)
                {
                    StartCoroutine(Shooting(2, 2));
                    StartCoroutine(Shooting(3, 0.5f));
                }
                else
                {
                    StartCoroutine(Shooting(1, 2.5f));
                    StartCoroutine(Shooting(2, 3));
                }
                break;

            case 2:
                if (rnd < 0.25f)
                {
                    StartCoroutine(Shooting(2, 5));
                    StartCoroutine(Shooting(3, 1.75f));
                }
                else if (rnd >= 0.25f && rnd < 0.75f)
                {
                    StartCoroutine(Shooting(1, 1.75f));
                    StartCoroutine(Shooting(2, 2.75f));
                }
                else
                {
                    StartCoroutine(Shooting(3, 5));
                }
                break;

            case 3:
                if (rnd <= 0.5f)
                {
                    StartCoroutine(Shooting(0, 6));
                }
                else
                {
                    StartCoroutine(Shooting(2, 3));
                    StartCoroutine(Shooting(3, 2.5f));
                }
                break;
            case 0:
                isDying = true;
                break;
        }

    }

    IEnumerator Shooting(int point, float time)
    {
        shootingPoints[point].SetActive(true);
        yield return new WaitForSeconds(time);
        shootingPoints[point].SetActive(false);
    }

    private void Dying()
    {
        isDying = true;
        onBossDying?.Invoke(bossRelatedDialogues);
        bossLoot.Loot(inventory.GetItems());
    }
}
