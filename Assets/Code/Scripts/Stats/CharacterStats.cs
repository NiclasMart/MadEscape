// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 24.09.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

namespace Stats
{
    public enum Stat
    {
        BaseDamage,
        PercentDamage,
        AttackSpeed,
        AttackRange,

        Life,
        LifeRegen,
        Armor,

        Sanity,
        SanityDecAmount,
        MovementSpeed,
        SanityConversionFactor,

        PillEfficiency
    }

    public class CharacterStats : MonoBehaviour
    {
        [SerializeField] BaseStats baseStatValues;
        private Dictionary<Stat, float> activeStats = new Dictionary<Stat, float>();

        public Action<Stat, float> onStatsChanged;

        private void Awake()
        {
            ReadInBaseStats();
        }

        public void SetNewStatData(BaseStats newStats)
        {
            baseStatValues = newStats;
            ReadInBaseStats();
        }

        public bool StatIsAvailable(Stat stat)
        {
            return activeStats.ContainsKey(stat);
        }

        //returns value of given stat, if stat is not available, return 0
        public float GetStat(Stat stat)
        {
            if (StatIsAvailable(stat))
            {
                return activeStats[stat];
            }
            else
            {
                Debug.LogWarning($"The stat {stat} on {gameObject.name}is used, but not set.");
                return 0;
            }
        }

        public void UpdateStat (Stat stat, float changeMultiplier)
        {
            if (StatIsAvailable(stat))
            {
                activeStats[stat] *= changeMultiplier;
                onStatsChanged.Invoke(stat, activeStats[stat]);
            }
            else
            {
                Debug.LogWarning($"The stat {stat} on {gameObject.name} could not be updated.");
            }
        }

        public void Clear()
        {
            activeStats.Clear();
        }

        void AddStatValue(Stat stat, float value)
        {
            if (StatIsAvailable(stat))
            {
                activeStats[stat] += value;
            }
            else
            {
                activeStats.Add(stat, value);
            }
        }

        void ReadInBaseStats()
        {
            if (baseStatValues == null)
                return;

            foreach (StatInputData statInput in baseStatValues.inputStats)
            {
                AddStatValue(statInput.stat, statInput.value);
                Debug.Log($"Added the stat {statInput.stat} with the value {statInput.value} to {gameObject.name}");
            }
        }
    }
}