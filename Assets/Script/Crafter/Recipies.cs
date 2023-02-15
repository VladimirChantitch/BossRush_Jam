using Boss.inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recepy")]
public class Recipies : ScriptableObject
{
    [SerializeField] AbstractItem item_1;
    [SerializeField] AbstractItem item_2;
    [SerializeField] AbstractItem result;

    public AbstractItem Item_1 { get => item_1; }
    public AbstractItem Item_2 { get => item_2; }
    public AbstractItem Result { get => result; }
}
