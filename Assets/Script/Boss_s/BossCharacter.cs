using Boss.inventory;
using Boss.loot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Boss.stats;
using Boss.Dialogue;
using System;
using Boss.save;

public class BossCharacter : AbstractCharacter
{
    [SerializeField] protected BossLoot bossLoot;
    [HideInInspector] public UnityEvent<BossRelatedDialogues> onBossDying = new UnityEvent<BossRelatedDialogues>();
    [HideInInspector] public UnityEvent<BossLootData> onBossDead = new UnityEvent<BossLootData>();
    [HideInInspector] public UnityEvent onBossHit = new UnityEvent();

    [SerializeField] BossUIManager bossUIManager;

    [SerializeField] protected BossRelatedDialogues bossRelatedDialogues;


    protected override void Init()
    {
        bossLoot.onLoot.AddListener(data => onBossDead?.Invoke(data));
        bossUIManager.InitBossHealth(GetStat(StatsType.health).Value, GetStat(StatsType.health).MaxValue);
    }

    public override void AddDamage(float amount)
    {
        base.AddDamage(amount);
        bossUIManager.SetBossHealth(GetStat(StatsType.health).Value, GetStat(StatsType.health).MaxValue);
    }
}

[Serializable]
public class BossRelatedDialogues
{
    [SerializeField] AbstractDialogue winDialogue;
    [SerializeField] AbstractDialogue looseDialogue;

    public AbstractDialogue WinDialogue { get => winDialogue; }
    public AbstractDialogue LooseDialogue { get => looseDialogue; }
}

public class BossRelated_Dto : DTO
{

}
