using Boss.inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static Boss.inventory.Inventory;

namespace Boss.UI
{
    public class UI_Inventory : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<UI_Inventory, UxmlTraits> { }

        List<UI_ItemSlot> uI_ItemSlots = new List<UI_ItemSlot>();

        public UnityEvent<AbstractItem> onItemSelected = new UnityEvent<AbstractItem>();
        public UnityEvent<AbstractItem> onItemDeselected = new UnityEvent<AbstractItem>();

        public UI_Inventory()
        {

        }

        internal void Init()
        {
            uI_ItemSlots = new List<UI_ItemSlot>();

            Children().ToList().ForEach(c => {
                uI_ItemSlots.Add(c as UI_ItemSlot);
            });

            uI_ItemSlots.ForEach(c =>
            {
                c.ItemSelected.AddListener(selected => onItemSelected?.Invoke(selected));
                c.ItemDeselected.AddListener(disSelected => onItemDeselected?.Invoke(disSelected));
            });
        }

        internal void CraftSuccess()
        {
            uI_ItemSlots.ForEach(c => {
                c.RemoveOverriderClass();
                c.isSelected = false;
            });
        }

        internal void DeselectItem(AbstractItem item)
        {
            uI_ItemSlots.Find(i => i.Item == item).RemoveOverriderClass();
        }

        internal void ClearAllSlots()
        {
            uI_ItemSlots.ForEach(c => c.Clean());
        }

        internal void SetItemSlots(List<AbstractItem> items)
        {
            ClearAllSlots();
            for (int i = 0; i < items.Count; i++)
            {
                uI_ItemSlots[i].Init(items[i]);
            }
        }
    }
}

