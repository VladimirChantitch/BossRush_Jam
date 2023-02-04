using Boss.inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DADDY : MonoBehaviour
{
    public static DADDY Instance { get; private set; }

    [SerializeField] public StyleSheet USS_STYLE; 

    [SerializeField] List<AbstractItem> itemsAvailible = new List<AbstractItem>();

    [SerializeField] List<Recipies> Recipies = new List<Recipies>();

    public AbstractItem GetItemByID(int ID)
    {
        AbstractItem item = itemsAvailible.Find(i => i.GetInstanceID() == ID);
        return item;
    }

    private void Awake()
    {
        Instance = this;
    }
}
