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

    [SerializeField] public BossRelatedDialogues bossRelatedDialogues;


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
    public BossRelatedDialogues() { }
    public BossRelatedDialogues(BossRelated_Dto dto)
    {
        this.winDialogue = DADDY.Instance.GetDialogueByID(dto.win_id);
        this.looseDialogue = DADDY.Instance.GetDialogueByID(dto.loose_id);
    }

    [SerializeField] AbstractDialogue winDialogue;
    [SerializeField] AbstractDialogue looseDialogue;

    public AbstractDialogue WinDialogue { get => winDialogue; }
    public AbstractDialogue LooseDialogue { get => looseDialogue; }

    public BossRelated_Dto Save()
    {
        if (looseDialogue == null || winDialogue == null)
        {
            return new BossRelated_Dto() { loose_id = -1, win_id = -1 };
        }
        else
        {
            return new BossRelated_Dto() { loose_id = LooseDialogue.GetInstanceID(), win_id = WinDialogue.GetInstanceID() };
        }

    }
}

public class BossRelated_Dto : DTO
{
    public int loose_id;
    public int win_id;
}
