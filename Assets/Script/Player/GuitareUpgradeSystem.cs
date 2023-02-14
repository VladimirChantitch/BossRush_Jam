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
        public UnityEvent<GuitareUpgrade> onUpgradeRemoved = new UnityEvent<GuitareUpgrade>();

        public void Load(GuitareUpgrade_DTO guitareUpgrade_DTO)
        {
            slots = new List<GuitareUpgradeSlot>();
            guitareUpgrade_DTO.guitareUpgradeSlots_DTOs.ForEach(slot_DTO=>
            { 
                slots.Add(new GuitareUpgradeSlot(slot_DTO));
            });

            onUpgradesUpdated?.Invoke(GetUpgrades());
            GuitareAspect guitareAspect = GameObject.FindObjectOfType<GuitareAspect>();  ///Stinks
            guitareAspect.UpdateGuitareAspect(GetUpgrades());   ///Stinks
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
            if(slots.Find(s => s.type == guitareUpgrade.UpgradePartType).RemoveUpgrade())
            {
                onUpgradeRemoved?.Invoke(guitareUpgrade);
            }
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
                this.guitareUpgrade = (GuitareUpgrade)DADDY.Instance.GetItemByID(dto.intance_ID);
            }

            public UpgradePartType type { get; private set; }
            public GuitareUpgrade guitareUpgrade { get; private set; }

            internal bool AddOrModifyUpgrade(GuitareUpgrade guitareUpgrade)
            {
                this.guitareUpgrade = guitareUpgrade;
                return true;
            }

            internal bool RemoveUpgrade()
            {
                this.guitareUpgrade = null;
                return true;
            }

            public GuitareUpgradeSlot_DTO Save()
            {
                if (guitareUpgrade == null)
                {
                    return new GuitareUpgradeSlot_DTO(type);
                }
                return new GuitareUpgradeSlot_DTO(type, guitareUpgrade.GetInstanceID());
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
        public GuitareUpgradeSlot_DTO(UpgradePartType type, int intance_ID = -1)
        {
            this.type = type;
            this.intance_ID = intance_ID;
        }

        public UpgradePartType type;
        public int intance_ID;
    }
}

