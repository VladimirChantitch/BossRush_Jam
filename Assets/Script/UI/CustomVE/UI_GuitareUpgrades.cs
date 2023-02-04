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

        List<UI_GuitareSlotUpgrades> uI_ItemSlots = new List<UI_GuitareSlotUpgrades>();
        public UnityEvent<GuitareUpgrade> onDisupgraded = new UnityEvent<GuitareUpgrade>();

        public UI_GuitareUpgrades()
        {
            Children().ToList().ForEach(c => uI_ItemSlots.Add(c as UI_GuitareSlotUpgrades));
        }

        internal void Init()
        {
            uI_ItemSlots.ForEach(c =>
            {
                c.onDisupgraded.AddListener(disSelected => onDisupgraded?.Invoke(disSelected));
            });
        }

        internal void SetInfo(List<GuitareUpgrade> guitareUpgrades)
        {
            uI_ItemSlots.ForEach(c => c.Clean());
            guitareUpgrades.ForEach(gu =>
            {
                uI_ItemSlots.Find(s => s.type == gu.UpgradePartType).Init(gu);
            });
        }

        internal void SetInfo(GuitareUpgrade guitareUpgrade)
        {
            UI_ItemSlot slot = uI_ItemSlots.Find(s => s.type == guitareUpgrade.UpgradePartType);
            slot.Init(guitareUpgrade);
        }
    }
}

