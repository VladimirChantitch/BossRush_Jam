using Boss.inventory;
using Boss.UI;
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
        [SerializeField] List<Recipies> recipies = new List<Recipies>();
        public UnityEvent<CrafterFailledDTO> failled = new UnityEvent<CrafterFailledDTO>();
        public UnityEvent<CrafterSuccessDTO> success = new UnityEvent<CrafterSuccessDTO>();

        AbstractItem[] items = new AbstractItem[2];

        public void HandleSelection(AbstractItem item)
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
                items[1] = items[0];
                items[0] = item;
            }

            if (items[1] != null && items[0] != null)
            {
                CheckRecipy();
            }
        }

        private void CheckRecipy()
        {
            Recipies firstSelected = recipies.Where(r => (r.Item_1 == items[0] || r.Item_1 == items[1]))
                                                .Where(r => (r.Item_2 == items[0] || r.Item_2 == items[1])).First();

            if (firstSelected == null)
            {
                FailledToCraft();
            }
            else
            {
                SucceedToCraft(firstSelected.Result);
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
            failled?.Invoke(new CrafterFailledDTO(items[0], items[1]));
        }

        private void SucceedToCraft(AbstractItem sacrifice)
        {
            success.Invoke(new CrafterSuccessDTO(items[0], items[1], sacrifice));
        }
    }
}


public class CrafterSuccessDTO
{
    public CrafterSuccessDTO(AbstractItem i1, AbstractItem i2, AbstractItem success)
    {
        item_1 = i1;
        item_2 = i2;
        resutl = success;
    }
    public AbstractItem item_1;
    public AbstractItem item_2;
    public AbstractItem resutl;
}

public class CrafterFailledDTO
{
    public CrafterFailledDTO(AbstractItem i1, AbstractItem i2)
    {
        item_1 = i1;
        item_2 = i2;
    }
    public AbstractItem item_1;
    public AbstractItem item_2;
}
