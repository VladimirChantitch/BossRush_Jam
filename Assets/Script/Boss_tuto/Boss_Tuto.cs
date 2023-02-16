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
        subBosses.ForEach(sb => sb.onKilled.AddListener(() =>
        {
            deadCount++;
            if (deadCount == subBosses.Count)
            {
                AllSubBossesDestroyed();
            }
        }));

        animator = GetComponent<Boss_1_Animator> ();

        gameObject.SetActive(false);
    }
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
        isPhase2 = true;
        isPhase1 = false;
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
            StopCoroutine(AwaitBeforeAttack());
            isAttacking = true;
            if (areSubDead)
            {
                HandleSubAttack();
            }
            else
            {
                HandleSoloAttack();
            }
        }
    }

    public void HandleSoloAttack()
    {
        if (isPhase2)
        {
            animator.PlayTargetAnimation(false, "Attaque", 0.5f);
        }
        else if (isPhase3)
        {
            animator.PlayTargetAnimation(false, "Attaque", 0.5f);
        }
    }

    private void HandleSubAttack()
    {
        if (isPhase2)
        {
            animator.PlayTargetAnimation(false, "Attaque", 0.5f);
        }
        else if (isPhase3)
        {
            animator.PlayTargetAnimation(false, "Attaque", 0.5f);
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
        StartCoroutine(ReviveAll());
        AddDamage(-1);
        if (isPhase1)
        {
            isPhase2 = true;
            isPhase1 = false;
            EnterAppearingState();
            gameObject.SetActive(true);
        }
        if (isPhase2)
        {
            isPhase2 = false;
            isPhase3 = true;
        }
    }

    IEnumerator ReviveAll()
    {
        areSubDead = false;
        yield return new WaitForSeconds(respawnTime);
        subBosses.ForEach(sb => sb.Revive());
        deadCount = 0;
        yield return null;
    }

    IEnumerator AwaitBeforeAttack()
    {
        yield return new WaitForSeconds(2.5f);
        EnterAttackState();
    }
}
