using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss.save;

namespace Boss.inventory
{
    [CreateAssetMenu(menuName = "Items/GuitareUpgrade")]
    public class GuitareUpgrade : AbstractItem
    {
        [SerializeField] UpgradePartType upgradePartType;

        public UpgradePartType UpgradePartType { get => upgradePartType; }
        public UpgradeGraphicsRefType upgradeGraphicsRefType;
    }
}


public enum UpgradePartType
{
    Head,
    Shaft,
    Body,
    TopBody,
    BottomBody,
    Amplification
}

public enum UpgradeGraphicsRefType
{
    Arm,
    Axe,
    Bone,
    Carrot,
    EggPlant,
    Eye,
    Horn,
    Lamp,
    Shell,
    Synth
}