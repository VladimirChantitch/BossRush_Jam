using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sub_BossTuto : BossCharacter
{
    public UnityEvent onKilled = new UnityEvent();
    [SerializeField] BossTakeDamageCollider takeDamageCollider;

    private void Awake()
    {
        takeDamageCollider.TakeDamageEvent.AddListener(value => AddDamage(value));
    }

    [SerializeField] bool isDead;
    [SerializeField] bool isShooting;
    [SerializeField] bool isIdle;

    enum SubBossState
    {
        Idle,
        Shooting,
        Dead
    }

    public bool isPhaseOne = true;
    public bool isPhaseTwo;
    public bool isPhaseThree;

    [SerializeField] SubBossState state = SubBossState.Idle;

    [SerializeField] Boss_1_Animator animator;

    private void Update()
    {
        CheckLife();
        HandleState();
    }

    private void CheckLife()
    {
        if (GetStat(Boss.stats.StatsType.health).Value <= 0)
        {
            EnterDeadState();
        }
    }

    private void HandleState()
    {
        switch (state)
        {
            case SubBossState.Idle:
                ExecuteIdleState();
                break;
            case SubBossState.Shooting:
                ExecuteAttackState();            
                break;
            case SubBossState.Dead:
                ExecuteDeadState();
                break;
        }
    }

    public void EnterIdleState()
    {
        state = SubBossState.Idle;
        isIdle = false;
    }

    private void ExecuteIdleState()
    {
        if (!isIdle)
        {
            isIdle = true;
            animator.PlayTargetAnimation(false, "Idle", 0.25f);

            if (!isPhaseOne)
            {
                StartCoroutine(AwaitBeforeAttack());
            }
        }
    }

    #region Attacks
    public void EnterAttackState()
    {
        if (isPhaseOne)
        {
            EnterIdleState();
        }
        else
        {
            state = SubBossState.Shooting;
            isShooting = false;
        }
    }

    private void ExecuteAttackState()
    {
        if (!isShooting)
        {
            isShooting = true;

            StopCoroutine(AwaitBeforeAttack());

            if (isPhaseTwo)
            {
                HandlePhaseTwoAttack();
                return;
            }

            if (isPhaseThree)
            {
                HandlePhaseThreeAttack();
            }
        }
    }

    private void HandlePhaseThreeAttack()
    {
        animator.PlayTargetAnimation(false, "Attaque", 0.25f);
    }

    private void HandlePhaseTwoAttack()
    {
        animator.PlayTargetAnimation(false, "Attaque", 0.25f);
    }
    #endregion

    public void EnterDeadState()
    {
        state = SubBossState.Dead;
        isDead = false;
    }

    private void ExecuteDeadState()
    {
        if (!isDead)
        {
            isDead = true;
            onKilled?.Invoke();
            gameObject.SetActive(false);
        }
    }

    internal void Revive()
    {
        gameObject.SetActive(true);
        SetStat(false, GetStat(Boss.stats.StatsType.health).MaxValue, Boss.stats.StatsType.health);
        EnterIdleState();
        if (isPhaseOne)
        {
            isPhaseOne = false;
            isPhaseTwo = true;
        }
        else if (isPhaseTwo)
        {
            isPhaseTwo = false;
            isPhaseThree = true;
        }
    }

    IEnumerator AwaitBeforeAttack()
    {
        yield return new WaitForSeconds(2.5f);
        EnterAttackState();
    }
}
