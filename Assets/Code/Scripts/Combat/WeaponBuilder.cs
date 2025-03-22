// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 19.11.24
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Combat
{
    public static class WeaponBuilder
    {
        static List<StatRecord> weaponData = null;
        public static Dictionary<Stat, float> GetWeaponStats(int weaponID)
        {
            weaponData ??= LoadStats.LoadWeaponStats();

            if (weaponData.Count - 1 >= weaponID)
            {
                return weaponData[weaponID].statDict;
            }
            else
            {
                Debug.LogWarning($"For the set weaponID {weaponID} no data is availabe. Loaded default weapon instead.");
                return weaponData[0].statDict;
            }
        }
    }
}