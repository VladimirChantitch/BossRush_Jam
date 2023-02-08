using Boss.inventory;
using Boss.loot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Boss.stats;

public class BossCharacter : AbstractCharacter
{
    [SerializeField] protected BossLoot bossLoot;
    [HideInInspector] public UnityEvent onBossDying = new UnityEvent();
    [HideInInspector] public UnityEvent<BossLootData> onBossDead = new UnityEvent<BossLootData>();
    [HideInInspector] public UnityEvent onBossHit = new UnityEvent();

    [SerializeField] BossUIManager bossUIManager;


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
