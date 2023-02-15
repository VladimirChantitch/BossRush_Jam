using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Boss.save;
using System.Linq;
using UnityEditor;
using System.IO;

namespace Boss.inventory
{
    [Serializable]
    [CreateAssetMenu(menuName = "Inventory")]
    public class Inventory : ScriptableObject
    {
        [SerializeField] List<ItemSlot> itemsSlots = new List<ItemSlot>();

        public Inventory Clone()
        {
            Inventory clone = (Inventory)CreateInstance($"{typeof(Inventory)}");
            itemsSlots.ForEach(i =>
            {
                clone.itemsSlots.Add(new ItemSlot(i.Amount, i.Item));
            });

            clone.name = "Clone_" + name;

            return clone;
        }

        public void Load(Inventory_DTO inventory_DTO)
        {
            itemsSlots.Clear();
            inventory_DTO.itemSlots.ForEach(itemSlot_dto =>
            {
                itemsSlots.Add(new ItemSlot(itemSlot_dto));
            });
        }

        public Inventory_DTO Save()
        {
            List<ItemSlot_DTO> itemSlot_DTOs = new List<ItemSlot_DTO>();
            itemsSlots.ForEach(itemSlot =>
            {
                ItemSlot_DTO slot_dto = itemSlot.Save();
                if (slot_dto.instance_ID != -1)
                {
                    itemSlot_DTOs.Add(slot_dto);
                }
            });

            return new Inventory_DTO(itemSlot_DTOs);
        }

        public void AddItem(AbstractItem item, int amount = 1)
        {
            ItemSlot itemSlot = itemsSlots.Find(itemSlot => itemSlot.Item == item);
            if (itemSlot != null)
            {
                itemSlot.AddItem(amount);
            }
            else
            {
                itemsSlots.Add(new ItemSlot(amount, item));
            }
        }

        public bool RemoveItem(AbstractItem item, int amount = -1)
        {
            if (item == null) return false;

            ItemSlot itemSlot = itemsSlots.Where(itemSlot => itemSlot.Item == item).First();
            if (itemSlot != null)
            {
                if (itemSlot.RemoveItem(amount))
                {
                    itemsSlots.Remove(itemSlot);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        internal List<AbstractItem> GetItems()
        {
            List<AbstractItem> items = new List<AbstractItem>();
            itemsSlots.ForEach(itemSlot =>
            {
                items.Add(itemSlot.Item);
            });
            return items;
        }

        [Serializable]
        public class ItemSlot
        {

            public ItemSlot (int amount, AbstractItem item)
            {
                this.amount = amount;
                this.item = item;
            }

            public ItemSlot (ItemSlot_DTO dto)
            {
                this.amount = dto.amount;
                this.item = DADDY.Instance.GetItemByID(dto.instance_ID);

            }

            [SerializeField] int amount;
            [SerializeField] AbstractItem item;

            public int Amount { get => amount; }
            public AbstractItem Item { get => item; }

            public void AddItem(int amount)
            {
                this.amount += amount;
            }

            public bool RemoveItem(int amount)
            {
                this.amount -= amount;
                if (this.amount <= 0)
                {
                    return true;
                }
                return false;
            }

            public ItemSlot_DTO Save()
            {
                if (Item == null)
                {
                    return new ItemSlot_DTO(amount);
                }
                return new ItemSlot_DTO(amount, Item.GetInstanceID());
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
        public ItemSlot_DTO(int amount, int instance_ID = -1)
        {
            this.amount = amount;
            this.instance_ID = instance_ID;
        }

        public int amount { private set; get; }
        public int instance_ID { private set; get; }
    }
}
