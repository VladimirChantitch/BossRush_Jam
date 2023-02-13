using Boss.inventory;
using Boss.UI;
using player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Boss.crafter
{
    public class Crafter : HubInteractor
    {
        List<Recipies> recipies = new List<Recipies>();
        public UnityEvent<CrafterData> onInfo = new UnityEvent<CrafterData>();
        public UnityEvent<CrafterData> onFail = new UnityEvent<CrafterData>();
        public UnityEvent<CrafterSuccessData> onSuccess = new UnityEvent<CrafterSuccessData>();
        public UnityEvent<AbstractItem> onDeselect = new UnityEvent<AbstractItem>();
        public UnityEvent onNoCraftAvailible = new UnityEvent();

        AbstractItem[] items = new AbstractItem[2];

        public void HandleSelection(AbstractItem item)
        {
            if (CheckIfPlayerCanEvenCraftSomething())
            {

                if (items == null) items = new AbstractItem[2];
                if (items[0] == null)
                {
                    items[0] = item;
                }
                else if (items[1] == null)
                {
                    items[1] = item;
                }
                else
                {
                    onDeselect?.Invoke(items[1]);
                    items[1] = items[0];
                    items[0] = item;
                }

                if (items[1] != null && items[0] != null)
                {
                    CheckRecipy();
                }

                onInfo?.Invoke(new CrafterData(items[0], items[1]));
            }
            else
            {
                onNoCraftAvailible?.Invoke();
            }
        }

        private bool CheckIfPlayerCanEvenCraftSomething()
        {
            List<AbstractItem> all_abstractItems = new List<AbstractItem>();
            all_abstractItems = FindObjectOfType<PlayerManager>().GetItems();
            if (all_abstractItems == null || all_abstractItems.Count == 0) return false;
            
            List<Recipies> firstselect_recipies = new List<Recipies>();
            all_abstractItems.ForEach(i =>
            {
                List<Recipies> local_recipies = recipies.Where(r => r.Item_1 == i).ToList();
                if (local_recipies != null || local_recipies.Count != 0)
                {
                    firstselect_recipies.AddRange(local_recipies);
                }
            });
            if (firstselect_recipies == null || firstselect_recipies.Count == 0) return false;

            List<Recipies> l = new List<Recipies>();
            l = firstselect_recipies.Distinct().ToList();

            List<Recipies> lastselect_recipies = new List<Recipies>();
            all_abstractItems.ForEach(i =>
            {
                List<Recipies> local_recipies = recipies.Where(r => r.Item_2 == i).ToList();
                if (local_recipies != null || local_recipies.Count != 0)
                {
                    lastselect_recipies.AddRange(local_recipies);
                }
            });

            if (lastselect_recipies == null || lastselect_recipies.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CheckRecipy()
        {
            List<Recipies> firstSelected = recipies.Where(r => (r.Item_1 == items[0] || r.Item_1 == items[1])).ToList();
            if (firstSelected == null) FailledToCraft();
            Recipies secondSelect = firstSelected.Find(r => (r.Item_2 == items[0] || r.Item_2 == items[1]));

            if (secondSelect == null)
            {
                FailledToCraft();
            }
            else
            {
                SucceedToCraft(secondSelect.Result);
            }
        }

        public void DeselectSlot(AbstractItem item)
        {
            if (items == null) return;

            if (items[0] == item)
            {
                items[0] = null;
            }
            else if (items[1] == item)
            {
                items[1] = null;
            }
        }

        private void FailledToCraft()
        {
            onFail?.Invoke(new CrafterData(items[0], items[1]));
        }

        private void SucceedToCraft(AbstractItem sacrifice)
        {
            onSuccess.Invoke(new CrafterSuccessData(items[0], items[1], sacrifice));
            items[0] = null;
            items[1] = null;
        }

        internal void Init(List<Recipies> recipies)
        {
            this.recipies = recipies;
        }
    }
}


public class CrafterSuccessData
{
    public CrafterSuccessData(AbstractItem i1, AbstractItem i2, AbstractItem success)
    {
        item_1 = i1;
        item_2 = i2;
        resutl = success;
    }
    public AbstractItem item_1;
    public AbstractItem item_2;
    public AbstractItem resutl;
}

public class CrafterData
{
    public CrafterData(AbstractItem i1, AbstractItem i2)
    {
        item_1 = i1;
        item_2 = i2;
    }
    public AbstractItem item_1;
    public AbstractItem item_2;
}
