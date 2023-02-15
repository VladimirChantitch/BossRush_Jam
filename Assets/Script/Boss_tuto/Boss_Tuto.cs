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
    }
    [SerializeField] bool isInvisible;
    [SerializeField] bool isAppearing;
    [SerializeField] bool isIdle;
    [SerializeField] bool isAttackSub;
    [SerializeField] bool isAttackSolo;
    [SerializeField] bool isDead;

    public bool isPhase1 = true;
    public bool isPhase2;
    public bool isPhase3;

    enum BossStates
    {
        Invisible,
        Appearing,
        Idle,
        Attack_withSub,
        Attack_Solo,
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
            case BossStates.Attack_withSub:
                ExecuteSubAttackState();
                break;
            case BossStates.Attack_Solo:
                ExecuteSoloAttackState();
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
        }
    }

    public void EnterSoloAttackState()
    {
        state = BossStates.Attack_Solo;
        isAttackSolo = false;
    }

    private void ExecuteSoloAttackState()
    {
        if (!isAttackSolo)
        {

        }
    }

    public void EnterSubAttackState()
    {
        state = BossStates.Attack_withSub;
        isAttackSub = false;
    }

    private void ExecuteSubAttackState()
    {
        if (!isAttackSub)
        {

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
        StartCoroutine(ReviveAll());
        EnterAppearingState();
        AddDamage(-1);

        /// The second time the main boss Attaks the pleyr with his vulneravle head
        /// The first time he goes full bullet hell
        throw new NotImplementedException();
    }

    IEnumerator ReviveAll()
    {
        yield return new WaitForSeconds(respawnTime);
        subBosses.ForEach(sb => sb.Revive());
        yield return null;
    }
}
