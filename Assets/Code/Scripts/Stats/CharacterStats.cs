// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 24.09.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public enum Stat
    {
        //Weapon Stats
        BaseDamage,
        AttackSpeed,
        AttackRange,
        BulletSpeed,
        Accuracy,
        BulletCount,

        //Character Stats
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
        private Dictionary<Stat, float> activeStats = new Dictionary<Stat, float>();

        public Action<Stat, float> OnStatsChanged;

        public void Initialize(Dictionary<Stat, float> baseStatDict)
        {
            SetNewStatData(baseStatDict);
        }

        public void SetNewStatData(Dictionary<Stat, float> newStats)
        {
            activeStats = new Dictionary<Stat, float>(newStats);

            foreach (KeyValuePair<Stat, float> stat in activeStats)
            {
                OnStatsChanged?.Invoke(stat.Key, stat.Value);
            }
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
                Debug.LogWarning($"The stat {stat} on {gameObject.name} is used, but not set.");
                return 0;
            }
        }

        public void SetStat(Stat stat, float value)
        {
            if (StatIsAvailable(stat))
            {
                activeStats[stat] = value;
            }
            else
            {
                Debug.LogWarning($"The stat {stat} on {gameObject.name} couldn't be updated.");

            }
        }

        public void MultipliStatValue(Stat stat, float changeMultiplier)
        {
            if (StatIsAvailable(stat))
            {
                activeStats[stat] *= changeMultiplier;
                OnStatsChanged.Invoke(stat, activeStats[stat]);
            }
            else
            {
                Debug.LogWarning($"The stat {stat} on {gameObject.name} could not be updated.");
            }
        }

        //Todo: should all classes that relie on stats also be resetted and the callback OnStatChanged resetted?
        public void Clear()
        {
            activeStats.Clear();
        }
    }
}