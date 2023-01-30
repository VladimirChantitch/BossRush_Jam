using Boss.loot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossCharacter : AbstractCharacter
{
    [SerializeField] BossLoot bossLoot;
    [HideInInspector] public UnityEvent<BossLootData> onLoot = new UnityEvent<BossLootData>();
    [HideInInspector] public UnityEvent onBossDead = new UnityEvent();

    protected void Init()
    {
        bossLoot.onLoot.AddListener(data => onLoot?.Invoke(data));
    }
}
