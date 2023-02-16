using Boss.stats;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Tuto : BossCharacter
{
    [SerializeField] List<Sub_BossTuto> subBosses = new List<Sub_BossTuto> ();

    public int deadCount;
    [SerializeField] float respawnTime;

    private void Awake()
    {
        subBosses.ForEach(sb => {

            sb.onKilled.AddListener(() =>
            {
                deadCount++;
                if (deadCount == subBosses.Count)
                {
                    AllSubBossesDestroyed();
                }
            });

            sb.onBossHit.AddListener(() => {
                onBossHit?.Invoke();
            });
        });

        animator = GetComponent<Boss_1_Animator> ();

        gameObject.SetActive(false);
    }

    [Header ("state")]
    [SerializeField] bool isInvisible;
    [SerializeField] bool isAppearing;
    [SerializeField] bool isIdle;
    [SerializeField] bool isAttacking;
    [SerializeField] bool areSubDead;
    [SerializeField] bool isDead;

    public bool isPhase1 = true;
    public bool isPhase2;
    public bool isPhase3;

    enum BossStates
    {
        Invisible,
        Appearing,
        Idle,
        Attack,
        Dead
    }

    [Header("Bullets")]
    [SerializeField] List<BulletSpawner> AttaqueBullets = new List<BulletSpawner>();
    [SerializeField] List<BulletSpawner> sub_02_bullets = new List<BulletSpawner>();
    [SerializeField] List<BulletSpawner> solo_01_bullets = new List<BulletSpawner>();
    [SerializeField] List<BulletSpawner> solo_02_bullets = new List<BulletSpawner>();

    public void CloseAllBulletSpawner()
    {
        AttaqueBullets.ForEach(b => b.gameObject.SetActive(false));
        sub_02_bullets.ForEach(b => b.gameObject.SetActive(false));
        solo_01_bullets.ForEach(b => b.gameObject.SetActive(false));
        solo_02_bullets.ForEach(b => b.gameObject.SetActive(false));
    }

    [SerializeField] BossStates state = BossStates.Invisible;
    [SerializeField] Boss_1_Animator animator;


    private void Update()
    {
        HandleState();
    }

    private void HandleState()
    {
        switch (state)
        {
            case BossStates.Invisible:
                break;
            case BossStates.Appearing:
                ExecuteAppearingState();
                break;
            case BossStates.Idle:
                ExecuteIdleState();
                break;
            case BossStates.Attack:
                ExecuteAttackState();
                break;
            case BossStates.Dead:
                ExecuteDeadState();
                break;
        }
    }

    public void EnterAppearingState()
    {
        isAppearing = false;
        state = BossStates.Appearing;
    }

    private void ExecuteAppearingState()
    {
        if (!isAppearing)
        {
            isAppearing = true;
            animator.PlayTargetAnimation(false, "Appear", 0.5f);
        }
    }

    public void EnterIdleState()
    {
        state = BossStates.Idle;
        isIdle = false;
    }

    private void ExecuteIdleState()
    {
        if (!isIdle)
        {
            isIdle = true;
            animator.PlayTargetAnimation(false, "Idle", 0.5f);
            StartCoroutine(AwaitBeforeAttack());
        }
    }

    public void EnterAttackState()
    {
        state = BossStates.Attack;
        isAttacking = false;
    }

    private void ExecuteAttackState()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            if (areSubDead)
            {
                HandleSoloAttack();
            }
            else
            {
                HandleSubAttack();
            }
        }
    }

    public void HandleSoloAttack()
    {
        if (isPhase2)
        {
            animator.PlayTargetAnimation(false, "Solo_01", 0.5f);
            solo_01_bullets.ForEach(b => b.gameObject.SetActive(true));
        }
        else if (isPhase3)
        {
            animator.PlayTargetAnimation(false, "Solo_02", 0.5f);
            solo_02_bullets.ForEach(b => b.gameObject.SetActive(true));
        }
    }

    private void HandleSubAttack()
    {
        if (isPhase2)
        {
            animator.PlayTargetAnimation(false, "Attaque", 0.5f);
            AttaqueBullets.ForEach(b => b.gameObject.SetActive(true));
        }
        else if (isPhase3)
        {
            animator.PlayTargetAnimation(false, "Sub_02", 0.5f);
            sub_02_bullets.ForEach(b => b.gameObject.SetActive(true));
        }
    }

    public void EnterDeadState()
    {
        state = BossStates.Dead;
        isDead = false;
    }

    private void ExecuteDeadState()
    {
        if (!isDead)
        {
            isDead = true;
        }
    }

    private void AllSubBossesDestroyed()
    {
        areSubDead = true;
        AddDamage(-1);
        if (isPhase1)
        {
            isPhase2 = true;
            isPhase1 = false;
            EnterAppearingState();
            gameObject.SetActive(true);
            StartCoroutine(ReviveAll());
        }
        else if (isPhase2)
        {
            isPhase2 = false;
            isPhase3 = true;
            StartCoroutine(ReviveAll());
        }
        else if (isPhase3)
        {
            StartCoroutine(BossDeath());
        }
    }

    IEnumerator ReviveAll()
    {
        yield return new WaitForSeconds(respawnTime);
        subBosses.ForEach(sb => sb.Revive());
        areSubDead = false;
        deadCount = 0;
    }

    IEnumerator AwaitBeforeAttack()
    {
        yield return new WaitForSeconds(2.5f);
        EnterAttackState();
    }

    IEnumerator BossDeath()
    {
        onBossDying?.Invoke(bossRelatedDialogues);
        yield return new WaitForSeconds(0.5f);
        bossLoot.Loot(GetItems());
    }
}
