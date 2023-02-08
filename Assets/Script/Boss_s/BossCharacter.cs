using Boss.inventory;
using Boss.loot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossCharacter : AbstractCharacter
{
    [SerializeField] protected BossLoot bossLoot;
    [HideInInspector] public UnityEvent<BossLootData> onBossDead = new UnityEvent<BossLootData>();

    private void Init()
    {
        bossLoot.onLoot.AddListener(data => onBossDead?.Invoke(data));
    }
}
