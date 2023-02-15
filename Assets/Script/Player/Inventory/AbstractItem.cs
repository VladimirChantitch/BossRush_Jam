using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss.save;
using Boss.Dialogue;

namespace Boss.inventory
{
    public class AbstractItem : ScriptableObject
    {
        [SerializeField] string itemName;
        [SerializeField] string description;
        [SerializeField] GameObject prefab;
        [SerializeField] Sprite icon;
        [SerializeField] AbstractDialogue descriptiveDialogue;

        public string ItemName { get => itemName; }
        public string Description { get => description; }
        public GameObject Prefab { get => prefab; }
        public Sprite Icon { get => icon; }
    }
}
