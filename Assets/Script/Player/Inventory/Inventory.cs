using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Boss.save;
using System.Linq;

namespace Boss.inventory
{
    [Serializable]
    [CreateAssetMenu(menuName = "Inventory")]
    public class Inventory : ScriptableObject
    {
        [SerializeField] List<ItemSlot> itemsSlots = new List<ItemSlot>();

        public void LoadData()
        {
            throw new NotImplementedException();
        }

        public Inventory_DTO SaveInventory()
        {
            throw new NotImplementedException();
        }

        public void AddItem(AbstractItem item, int amount = 1)
        {
            ItemSlot itemSlot = itemsSlots.Where(itemSlot => itemSlot.Item == item).First();
            if (itemSlot != null)
            {
                itemSlot.AddItem(amount);
            }
            else
            {
                itemsSlots.Add(new ItemSlot(amount, item));
            }
        }

        [Serializable]
        public class ItemSlot
        {

            public ItemSlot (int amount, AbstractItem item)
            {
                this.amount = amount;
                this.item = item;
            }

            [SerializeField] int amount;
            [SerializeField] AbstractItem item;

            public int Amount { get => amount; }
            public AbstractItem Item { get => item; }

            public void AddItem(int amount)
            {
                this.amount = amount;
            }
        }
    }

    public class Inventory_DTO : DTO
    {
        public Inventory_DTO(List<ItemSlot_DTO> itemSlots)
        {
            this.itemSlots = itemSlots;
        }

        public List<ItemSlot_DTO> itemSlots;
    }

    public class ItemSlot_DTO : DTO
    {
        public ItemSlot_DTO(int amount, int id)
        {
            this.amount = amount;
            this.id = id;
        }

        public int amount { private set; get; }
        public int id { private set; get; }
    }
}
