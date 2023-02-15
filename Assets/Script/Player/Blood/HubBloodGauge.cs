using Boss.UI;
using player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class HubBloodGauge : HubInteractor
{
    [SerializeField] Transform gaugeTranform;
    
    /// <summary>
    /// Send an amount between 0 and 1
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateAmount(float currentAmount)
    {
        Vector3 new_scale = gaugeTranform.localScale;
        new_scale.y = math.remap(0f, 1000f, 0f, 13.33f, currentAmount);
        gaugeTranform.localScale = new_scale;
    }

    internal void Init()
    {
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        (float,float) value = (playerManager.GetStat(Boss.stats.StatsType.Blood).Value, playerManager.GetStat(Boss.stats.StatsType.Blood).MaxValue);
        UpdateAmount(value.Item1);
    }


}
