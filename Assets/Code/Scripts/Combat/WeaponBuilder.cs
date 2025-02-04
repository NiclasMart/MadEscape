// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 19.11.24
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Combat
{
    public static class WeaponBuilder
    {
        public static Weapon ConfigureWeapon(Weapon weapon, WeaponTemplate weaponConfig, string targetLayer)
        {
            //load weapon stats from file
            int weaponID = (int)weaponConfig.WeaponID;
            List<StatRecord> loadedStatData = LoadStats.LoadWeaponStats();
            Dictionary<Stat, float> baseStats;
            if (loadedStatData.Count - 1 < weaponID)
            {
                baseStats = loadedStatData[0].statDict;
                Debug.LogWarning("For the set weaponID no data is availabe. Loaded default weapon insted.");
            }
            else
            {
                baseStats = loadedStatData[weaponID].statDict;
            }

            weapon.Initialize(baseStats, weaponConfig.BulletColor, targetLayer);
            return weapon;
        }
    }
}