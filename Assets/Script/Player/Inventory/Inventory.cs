using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Boss.save;
using System.Linq;
using static Boss.inventory.Inventory;
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
            Inventory clone = new Inventory();
            clone.itemsSlots.AddRange(itemsSlots);

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
                itemSlot_DTOs.Add(itemSlot.Save());
            });

            return new Inventory_DTO(itemSlot_DTOs);
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

        public bool RemoveItem(AbstractItem item, int amount = -1)
        {
            ItemSlot itemSlot = itemsSlots.Where(itemSlot => itemSlot.Item == item).First();
            if (itemSlot != null)
            {
                itemSlot.RemoveItem(amount);
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
                this.item = (AbstractItem)AssetDatabase.LoadAssetAtPath(dto.path, typeof(AbstractItem));

            }

            [SerializeField] int amount;
            [SerializeField] AbstractItem item;

            public int Amount { get => amount; }
            public AbstractItem Item { get => item; }

            public void AddItem(int amount)
            {
                this.amount += amount;
            }

            public void RemoveItem(int amount)
            {
                this.amount -= amount;
            }

            public ItemSlot_DTO Save()
            {
                string path = AssetDatabase.GetAssetPath(Item);

                return new ItemSlot_DTO(amount, path);
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
        public ItemSlot_DTO(int amount, string path)
        {
            this.amount = amount;
            this.path = path;
        }

        public int amount { private set; get; }
        public string path { private set; get; }
    }
}
