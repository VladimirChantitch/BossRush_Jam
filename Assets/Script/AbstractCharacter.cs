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

    public void AddToInventory(AbstractItem item)
    {
        if (inventory == null)
        {
            throw new NotImplementedException();
        }
    }

    public void RemoveItem()
    {
        if (inventory == null)
        {
            throw new NotImplementedException();
        }
    }

    public AbstractItem GetItem(AbstractItem item)
    {
        throw new NotImplementedException();
    }

    #endregion
}


