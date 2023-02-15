using Boss.save;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.stats
{
    [Serializable]
    public class Stat
    {
        public Stat(float Value, float maxValue, StatsType statType)
        {
            this.value = Value;
            this.maxValue = maxValue;
            this.statType = statType;
        }

        [SerializeField] float value;
        [SerializeField] float maxValue;
        [SerializeField] StatsType statType;
        public float Value { get => value; }
        public float MaxValue { get => maxValue; }
        public StatsType StatType { get { return statType; } }

        public void SetStat(bool isMax, float value)
        {
            if (isMax)
            {
                this.maxValue = value;
            }
            else
            {
                this.value = value;
                if (this.value > maxValue)
                {
                    this.value = maxValue;
                }
                else if (this.value < 0)
                {
                    this.value = 0;
                }
            }
        }

        public void AddStat(bool isMax, float amount)
        {
            if (isMax)
            {
                this.maxValue += amount;
            }
            else
            {
                this.value += amount;
                if (value > maxValue)
                {
                    value = maxValue;
                }
                else if (value < 0)
                {
                    value = 0;
                }
            }
        }
    }

    public enum StatsType
    {
        health,
        dash,
        rythme_deadZone,
        combo,
        Blood
    }

    public class Stat_DTO : DTO
    {
        public Stat_DTO(float Value, float maxValue, StatsType statType)
        {
            this.value = Value;
            this.maxValue = maxValue;
            this.statType = statType;
        }

        public float value { get; private set; }
        public float maxValue { get; private set; }
        public StatsType statType { get; private set; }
    }
}

