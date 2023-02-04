using Boss.inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Boss.UI
{
    public class UI_Crafter : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<UI_Crafter, UxmlTraits> { }

        List<UI_ItemSlot> uI_ItemSlots = new List<UI_ItemSlot>();

        public UnityEvent<AbstractItem> onItemDeselected = new UnityEvent<AbstractItem>();

        public UI_Crafter()
        {

        }

        internal void Init()
        {
            uI_ItemSlots = new List<UI_ItemSlot>();

            Children().ToList().ForEach(c => uI_ItemSlots.Add(c as UI_ItemSlot));

            uI_ItemSlots.ForEach(c =>
            {
                c.Clean();
                c.ItemSelected.AddListener(t => Debug.Log("nothing, but thats ok"));
                c.ItemDeselected.AddListener(disSelected => onItemDeselected?.Invoke(disSelected));
            });
        }

        internal void Fail()
        {
            uI_ItemSlots.ForEach(c => c.Clean());
        }

        internal void Info(CrafterData dto)
        {
            uI_ItemSlots.ForEach(c => c.Clean());
            if (dto.item_1 != null) uI_ItemSlots[0].Init(dto.item_1);
            if (dto.item_2 != null) uI_ItemSlots[1].Init(dto.item_2);
        }

        public void Deselect(AbstractItem item)
        {
            uI_ItemSlots.Find(s => s.Item == item).Clean();
        }

        internal void CraftSuccess()
        {
            uI_ItemSlots.ForEach(c => c.Clean());
        }
    }
}

