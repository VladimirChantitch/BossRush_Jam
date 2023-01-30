using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss.save;

namespace Boss.inventory
{
    public class AbstractItem : ScriptableObject
    {
        [SerializeField] string itemName;
        [SerializeField] string description;
        [SerializeField] GameObject prefab;
        [SerializeField] Sprite icon;

        public string ItemName { get => itemName; }
        public string Description { get => description; }
        public GameObject Prefab { get => prefab; }
        public Sprite Icon { get => icon; }
    }
}
