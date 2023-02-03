using Boss.inventory;
using Boss.Upgrades.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Upgrades
{
    public class GuitareAspect : MonoBehaviour
    {
        List<UpgradeSlots> slots = new List<UpgradeSlots>();

        public void UpdateGuitareAspect(List<GuitareUpgrade> guitareUpgrades)
        {
            guitareUpgrades.ForEach(gu =>
            {
                ChangeSprite(gu.Icon, gu.UpgradePartType);
            });
        }

        public void UpdateGuitareAspect(GuitareUpgrade guitareUpgrade)
        {
            ChangeSprite(guitareUpgrade.Icon, guitareUpgrade.UpgradePartType);
        }

        private void ChangeSprite(Sprite sprite, UpgradePartType upgradePart)
        {
            slots.Find(s => s.upgrade == upgradePart).LoadNewUpgrade(sprite);
        }

        [Serializable]
        protected class UpgradeSlots
        {
            public UpgradeSlots(UpgradePartType upgrade, SpriteRenderer spriteRenderer)
            {
                this.spriteRenderer = spriteRenderer;
                this.upgrade = upgrade;
            }

            public UpgradePartType upgrade;
            [SerializeField] SpriteRenderer spriteRenderer = null;

            public void LoadNewUpgrade(Sprite sprite)
            {
                spriteRenderer.sprite = sprite;
            }
        }
    }
}

