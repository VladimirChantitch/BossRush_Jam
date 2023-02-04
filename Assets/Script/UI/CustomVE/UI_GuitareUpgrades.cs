using Boss.inventory;
using Boss.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Boss.Upgrades.UI
{
    public class UI_GuitareUpgrades : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<UI_GuitareUpgrades, UxmlTraits> { }

        List<UI_GuitareSlotUpgrades> uI_UpgradeSlots = new List<UI_GuitareSlotUpgrades>();
        public UnityEvent<GuitareUpgrade> onDisupgraded = new UnityEvent<GuitareUpgrade>();

        public UI_GuitareUpgrades()
        {

        }

        internal void Init()
        {
            Children().ToList().ForEach(c => uI_UpgradeSlots.Add(c as UI_GuitareSlotUpgrades));
            int index = 0;
            uI_UpgradeSlots.ForEach(c =>
            {
                c.onDisupgraded.AddListener(disSelected => onDisupgraded?.Invoke(disSelected));
                c.type = (UpgradePartType)index;
                index++;    
            });
        }

        internal void SetInfo(List<GuitareUpgrade> guitareUpgrades)
        {
            uI_UpgradeSlots.ForEach(c => c.Clean());
            guitareUpgrades.ForEach(gu =>
            {
                uI_UpgradeSlots.Find(s => s.type == gu.UpgradePartType).Init(gu);
            });
        }

        internal void SetInfo(GuitareUpgrade guitareUpgrade)
        {
            UI_GuitareSlotUpgrades slot = uI_UpgradeSlots.Find(s => s.type == guitareUpgrade.UpgradePartType);
            slot.Init(guitareUpgrade);
        }
    }
}

