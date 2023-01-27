using Boss.inventory;
using Boss.save;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Boss.stats;

public abstract class AbstractCharacter : MonoBehaviour
{
    private void Start()
    {
        if (inventory != null)
        {
            inventory = inventory.Clone();
        }
    }
    #region stats
    [SerializeField] protected List<Stat> stats = new List<Stat>()
    {
        new Stat(100, 100, StatsType.health),
    };

    /// <summary>
    /// take damage or heals character
    /// </summary>
    /// <param name="amout"></param>
    public virtual void AddDamage(float amount)
    {
        Debug.Log($"<color=purple> {gameObject.name} has taken {amount} damages </color>");
        stats.Where(s => s.StatType == StatsType.health).First().AddStat(false, amount);
    }

    public void SetStat(bool isMax, float value, StatsType statsType)
    {
        stats.Where(s => s.StatType == statsType).First().SetStat(isMax, value);
    }

    public Stat GetStat(StatsType statsType)
    {
        return stats.Where(s => s.StatType == statsType).FirstOrDefault();
    }

    #endregion

    #region Inventory
    [SerializeField] protected Inventory inventory;

    public void AddToInventory(AbstractItem item, int amount = 1)
    {
        if (inventory != null)
        {
            inventory.AddItem(item, amount);
        }
    }

    public bool RemoveItem(AbstractItem item, int amount = -1)
    {
        if (inventory != null)
        {
            if (inventory.RemoveItem(item, amount))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public List<AbstractItem> GetItems()
    {
        if (inventory != null)
        {
            return inventory.GetItems();
        }

        return new List<AbstractItem>();
    }

    #endregion
}


