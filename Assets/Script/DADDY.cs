using Boss.inventory;
using Boss.Map;
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
    }

    [Serializable]
    public class BossFight
    {
        public string BossFightName;
        public BossLocalization location;
    }
}
