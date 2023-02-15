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

    public bool isPhaseOne;
    public bool isPhaseTwo;
    public bool isPhaseThree;

    [SerializeField] SubBossState state;

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
            state = SubBossState.Dead;
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
                if (!isPhaseOne)
                {
                    ExecuteAttackState();
                }
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
            animator.PlayTargetAnimation(true, "Idle", 0.25f);
        }
    }

    #region Attacks
    public void EnterAttackState()
    {
        state = SubBossState.Shooting;
        isShooting = false;
    }

    private void ExecuteAttackState()
    {
        if (!isShooting)
        {
            isShooting = true;

            animator.PlayTargetAnimation(true, "Attack", 0.25f);
            if (isPhaseTwo)
            {
                HandlePhaseTwoAttack();
                return;
            }

            HandlePhaseThreeAttack();
        }
    }

    private void HandlePhaseThreeAttack()
    {
        throw new NotImplementedException();
    }

    private void HandlePhaseTwoAttack()
    {
        throw new NotImplementedException();
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
            onBossDying?.Invoke(null);
            ///Trigger a small explosion
        }
    }

    internal void Revive()
    {
        throw new NotImplementedException();
    }
}
