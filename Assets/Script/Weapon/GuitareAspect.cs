using Boss.inventory;
using Boss.Upgrades.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Boss.Upgrades
{
    public class GuitareAspect : MonoBehaviour
    {
        List<GuitareAspectSlot> slots = new List<GuitareAspectSlot>();

        private void Awake()
        {
            slots = GetComponentsInChildren<GuitareAspectSlot>().ToList();
        }

        public void UpdateGuitareAspect(List<GuitareUpgrade> guitareUpgrades)
        {
            guitareUpgrades.ForEach(gu =>
            {
                ChangeSprite(gu.Icon, gu.UpgradePartType);
            });
        }

        public void UpdateGuitareAspect(GuitareUpgrade guitareUpgrade)
        {
            if (guitareUpgrade != null)
            {
                ChangeSprite(guitareUpgrade.Icon, guitareUpgrade.UpgradePartType);
            }
        }

        private void ChangeSprite(Sprite sprite, UpgradePartType upgradePart)
        {
            slots.Find(s => s.type == upgradePart).LoadNewUpgrade(sprite);
        }
    }
}

