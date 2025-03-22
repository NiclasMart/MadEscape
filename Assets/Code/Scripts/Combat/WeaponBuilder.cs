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
        static List<StatRecord> weaponData = null;
        public static Weapon ConfigureWeapon(Weapon weapon, WeaponTemplate weaponConfig, string targetLayer)
        {
            //load weapon stats from file
            int weaponID = (int)weaponConfig.WeaponID;
            weaponData ??= LoadStats.LoadWeaponStats();

            Dictionary<Stat, float> baseStats;
            if (weaponData.Count - 1 >= weaponID)
            {
                baseStats = weaponData[weaponID].statDict;
            }
            else
            {
                baseStats = weaponData[0].statDict;
                Debug.LogWarning($"For the set weaponID {weaponID} no data is availabe. Loaded default weapon instead.");
            }

            weapon.Initialize(baseStats, weaponConfig.BulletColor, targetLayer);
            return weapon;
        }
    }
}