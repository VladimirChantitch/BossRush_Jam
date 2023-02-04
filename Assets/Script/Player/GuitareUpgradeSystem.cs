using Boss.inventory;
using Boss.save;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Boss.Upgrades
{
    public class GuitareUpgradeSystem
    {
        List<GuitareUpgradeSlot> slots = new List<GuitareUpgradeSlot>
        {
            new GuitareUpgradeSlot(UpgradePartType.Head, null),
            new GuitareUpgradeSlot(UpgradePartType.Amplification, null),
            new GuitareUpgradeSlot(UpgradePartType.Body, null),
            new GuitareUpgradeSlot(UpgradePartType.BottomBody, null),
            new GuitareUpgradeSlot(UpgradePartType.TopBody, null),
            new GuitareUpgradeSlot(UpgradePartType.Shaft, null)
        };

        public UnityEvent<List<GuitareUpgrade>> onUpgradesUpdated = new UnityEvent<List<GuitareUpgrade>>();

        public void Load(GuitareUpgrade_DTO guitareUpgrade_DTO)
        {
            slots = new List<GuitareUpgradeSlot>();
            guitareUpgrade_DTO.guitareUpgradeSlots_DTOs.ForEach(slot_DTO=>
            { 
                slots.Add(new GuitareUpgradeSlot(slot_DTO));
            });
        }

        public GuitareUpgrade_DTO Save()
        {
            List<GuitareUpgradeSlot_DTO> upgradeSlots_DTOs = new List<GuitareUpgradeSlot_DTO>();
            slots.ForEach(upgradeSlot =>
            {
                upgradeSlots_DTOs.Add(upgradeSlot.Save());
            });

            return new GuitareUpgrade_DTO(upgradeSlots_DTOs);
        }

        /// <summary>
        /// Get all the upgrades present on the guitare
        /// </summary>
        /// <returns></returns>
        public List<GuitareUpgrade> GetUpgrades()
        {
            List<GuitareUpgrade> upgrades = new List<GuitareUpgrade>();

            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].guitareUpgrade != null)
                {
                    upgrades.Add(slots[i].guitareUpgrade);
                }
            }

            return upgrades;
        }

        /// <summary>
        /// Adds new upgrades or modify old ones 
        /// </summary>
        /// <param name="guitareUpgrade"></param>
        public void AddOrModdifyUpgrade(GuitareUpgrade guitareUpgrade)
        {
            if(slots.Find(s => s.type == guitareUpgrade.UpgradePartType).AddOrModifyUpgrade(guitareUpgrade))
            {
                onUpgradesUpdated?.Invoke(GetUpgrades());
            }
        }

        public void RemoveUpgrade(GuitareUpgrade guitareUpgrade)
        {
            slots.Find(s => s.type == guitareUpgrade.UpgradePartType).RemoveUpgrade();
        }

        public class GuitareUpgradeSlot
        {
            public GuitareUpgradeSlot(UpgradePartType type, GuitareUpgrade guitareUpgrade)
            {
                this.type = type;
                this.guitareUpgrade = guitareUpgrade;
            }

            public GuitareUpgradeSlot(GuitareUpgradeSlot_DTO dto)
            {
                this.type = dto.type;
                this.guitareUpgrade = (GuitareUpgrade)AssetDatabase.LoadAssetAtPath(dto.path, typeof(GuitareUpgrade));
            }

            public UpgradePartType type { get; private set; }
            public GuitareUpgrade guitareUpgrade { get; private set; }

            internal bool AddOrModifyUpgrade(GuitareUpgrade guitareUpgrade)
            {
                this.guitareUpgrade = guitareUpgrade;
                return true;
            }

            internal void RemoveUpgrade()
            {
                this.guitareUpgrade = null;
            }

            public GuitareUpgradeSlot_DTO Save()
            {
                string path = AssetDatabase.GetAssetPath(guitareUpgrade);
                return new GuitareUpgradeSlot_DTO(type, path);
            }
        }
    }

    public class GuitareUpgrade_DTO : DTO
    {
        public GuitareUpgrade_DTO(List<GuitareUpgradeSlot_DTO> guitareUpgradeSlot_DTOs)
        {
            this.guitareUpgradeSlots_DTOs = guitareUpgradeSlot_DTOs;
        }

        public List<GuitareUpgradeSlot_DTO> guitareUpgradeSlots_DTOs = new List<GuitareUpgradeSlot_DTO>();
    }

    public class GuitareUpgradeSlot_DTO : DTO
    {
        public GuitareUpgradeSlot_DTO(UpgradePartType type, string path)
        {
            this.type = type;
            this.path = path;
        }

        public UpgradePartType type;
        public string path;
    }
}

