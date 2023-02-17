using Boss.inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Boss.loot
{
    public class BossLoot : MonoBehaviour
    {
        List<BossItem> bossItems = new List<BossItem>();
        List<GuitareUpgrade> guitareUpgrades = new List<GuitareUpgrade>();
        [HideInInspector] public UnityEvent<BossLootData> onLoot = new UnityEvent<BossLootData>();

        /// <summary>
        /// Select the items to loot in the inventory
        /// </summary>
        /// <param name="items"></param>
        public void Loot(List<AbstractItem> items)
        {
            items.ForEach(i =>
            {
                switch (i)
                {
                    case BossItem bl:
                        bossItems.Add(bl);
                        break;
                    case GuitareUpgrade gu:
                        guitareUpgrades.Add(gu);
                        break;
                }
            });

            while (bossItems.Count > 2)
            {
                bossItems.RemoveAt(Random.Range(0, bossItems.Count));
            }

            while (guitareUpgrades.Count > 1)
            {
                guitareUpgrades.RemoveAt(Random.Range(0, guitareUpgrades.Count));
            }
            LaunchLootMoment();
        }

        private void LaunchLootMoment()
        {
            onLoot?.Invoke(new BossLootData(bossItems, guitareUpgrades));
        }
    }

    public class BossLootData
    {
        public BossLootData(List<BossItem> bossItems, List<GuitareUpgrade> guitareUpgrades)
        {
            this.bossItems = bossItems;
            this.guitareUpgrades = guitareUpgrades;
        }

        public List<BossItem> bossItems = new List<BossItem>();
        public List<GuitareUpgrade> guitareUpgrades = new List<GuitareUpgrade>();
    }
}

