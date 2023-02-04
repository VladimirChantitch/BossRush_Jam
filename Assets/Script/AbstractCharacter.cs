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
    protected virtual void Init()
    {
        if (inventory != null)
        {
            inventory = inventory.Clone();
        }
        else
        {
            inventory = (Inventory)ScriptableObject.CreateInstance($"{typeof(Inventory)}");
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

    /// <summary>
    /// Remove a specfic item from the inventory
    /// </summary>
    /// <param name="item">item to remove</param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool RemoveFromInventory(AbstractItem item, int amount = 1)
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

    /// <summary>
    /// Get the items in the player inventory
    /// </summary>
    /// <returns></returns>
    public List<AbstractItem> GetItems()
    {
        if (inventory != null)
        {
            return inventory.GetItems();
        }

        return new List<AbstractItem>();
    }

    /// <summary>
    /// Add looted items to player inventory
    /// </summary>
    /// <param name="guitareUpgrades"></param>
    /// <param name="bossItems"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void AddToInventory(List<GuitareUpgrade> guitareUpgrades, List<BossItem> bossItems)
    {
        guitareUpgrades.ForEach(gu =>
        {
            AddToInventory(gu);
        });

        bossItems.ForEach(bi =>
        {
            AddToInventory(bi);
        });
    }


    public void AddToInventory(AbstractItem item, int amount = 1)
    {
        if (inventory != null)
        {
            inventory.AddItem(item, amount);
        }
    }

    #endregion
}


