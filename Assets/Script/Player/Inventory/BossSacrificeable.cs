using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Boss.inventory
{
    [CreateAssetMenu(menuName = "Items/BossSacrifice")]
    public class BossSacrificeable : AbstractItem
    {
        [SerializeField] SceneAsset BossFight;
    }
}

