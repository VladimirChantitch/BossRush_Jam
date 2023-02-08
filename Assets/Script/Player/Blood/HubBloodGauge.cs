using Boss.UI;
using player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubBloodGauge : HubInteractor
{
    [SerializeField] Transform gaugeTranform;
    
    /// <summary>
    /// Send an amount between 0 and 1
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateAmount(float amount)
    {
        Vector3 new_scale = gaugeTranform.localScale;
        new_scale.y = amount;
        gaugeTranform.localScale = new_scale;
    }

    internal void Init()
    {
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        (float,float) value = (playerManager.GetStat(Boss.stats.StatsType.Blood).Value, playerManager.GetStat(Boss.stats.StatsType.Blood).MaxValue);
        UpdateAmount(value.Item1 / value.Item2);
    }
}
