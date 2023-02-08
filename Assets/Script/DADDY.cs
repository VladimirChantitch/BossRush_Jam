using Boss.Dialogue;
using Boss.inventory;
using Boss.Map;
using Boss.save;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DADDY : MonoBehaviour, ISavable
{
    public static DADDY Instance { get; private set; }

    [SerializeField] public StyleSheet USS_STYLE; 

    [SerializeField] List<AbstractItem> itemsAvailible = new List<AbstractItem>();

    [SerializeField] List<Recipies> Recipies = new List<Recipies>();

    [SerializeField] List<BossFight> bossFights = new List<BossFight> ();

    public AbstractItem GetItemByID(int ID)
    {
        AbstractItem item = itemsAvailible.Find(i => i.GetInstanceID() == ID);
        return item;
    }

    public string GetBossFight(BossLocalization location)
    {
        return bossFights.Find(bf => bf.location == location).BossFightName;
    }

    private void Awake()
    {
        Instance = this;
        hasJustLoose = false;
    }

    public bool hasJustLoose;


    [Serializable]
    public class BossFight
    {
        public string BossFightName;
        public BossLocalization location;
    }

    public AbstractDialogue GetANewDialogue(string tag, DialogueActionType type, ActionTypes actionTypes)
    {
        AbstractDialogue dialogue = allDialogues.Find(a => a.title == tag && a.type == type && a.actionType == actionTypes);
        if (dialogue != null)
        {
            unlockedDialogues.Add(dialogue);
        }

        return dialogue;
    }

    List<AbstractDialogue> allDialogues = new List<AbstractDialogue>();
    List<AbstractDialogue> unlockedDialogues = new List<AbstractDialogue>();
    public List<AbstractDialogue> GetUnlockedDialogues()
    {
        List<AbstractDialogue> list = new List<AbstractDialogue>();
        list.AddRange(unlockedDialogues);
        return list;
    }

    public DTO GetData()
    {
        List<int> list = new List<int>();
        unlockedDialogues.ForEach(d => list.Add(d.GetInstanceID()));
        return new Daddy_DTO() { unlockedDialogues = list };
    }

    public void LoadData(DTO dTO)
    {
        (dTO as Daddy_DTO).unlockedDialogues.ForEach(d =>
        {
            unlockedDialogues.Add(allDialogues.Find(al => al.GetInstanceID() == d));
        });
    }
}

[Serializable]
public class Daddy_DTO : DTO
{
    public List<int> unlockedDialogues = new List<int>();
}

