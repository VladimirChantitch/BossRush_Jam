using Boss.Dialogue;
using Boss.inventory;
using Boss.Map;
using Boss.save;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DADDY : MonoBehaviour
{
    public static DADDY Instance { get; private set; }

    [SerializeField] public StyleSheet USS_STYLE;

    [SerializeField] List<AbstractItem> itemsAvailible = new List<AbstractItem>();

    [SerializeField] List<Recipies> Recipies = new List<Recipies>();

    [SerializeField] List<BossFight> bossFights = new List<BossFight>();

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

    public List<AbstractDialogue> allDialoguesThatNeedToBeSaved = new List<AbstractDialogue>();

    public AbstractDialogue GetDialogueByID(int ID)
    {
        AbstractDialogue dialogue = allDialoguesThatNeedToBeSaved.Find(i => i.GetInstanceID() == ID);
        return dialogue;
    }

    public List<AudioClip> clips = new List<AudioClip>();

    internal AudioClip GetClipByName(string clip_name)
    {
        return clips.Find(c => c.name == clip_name);
    }
}

