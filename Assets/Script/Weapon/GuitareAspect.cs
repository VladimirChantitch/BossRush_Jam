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
        List<UpgradeGraphicsRefType> unlocked = new List<UpgradeGraphicsRefType>();

        private void Awake()
        {
            slots = GetComponentsInChildren<GuitareAspectSlot>().ToList();
        }

        public void UpdateGuitareAspect(List<GuitareUpgrade> guitareUpgrades, bool isTrue = false)
        {
            if (isTrue) unlocked.Clear();

            List<UpgradeGraphicsRefType> localUnlock = new List<UpgradeGraphicsRefType>();
            localUnlock.AddRange(unlocked);

            guitareUpgrades.ForEach(gu =>
            {
                localUnlock.Add(gu.upgradeGraphicsRefType);
            });

            unlocked.Clear();
            unlocked.AddRange(localUnlock.Distinct());

            ReloadSprites();
        }

        public void DisUpdateGuitareApsect(List<GuitareUpgrade> guitareUpgrades)
        {
            guitareUpgrades.ForEach(gu => unlocked.Remove(gu.upgradeGraphicsRefType));
            ReloadSprites();
        }

        private void ReloadSprites()
        {
            slots.ForEach(s =>
            {
                if (unlocked.Contains(s.type))
                {
                    s.EnableUpgrade();
                }
                else
                {
                    s.DisableUpgrade();
                }
            });
        }
    }
}

