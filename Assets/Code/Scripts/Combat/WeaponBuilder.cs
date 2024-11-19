// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 19.11.24
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Combat
{
    public static class WeaponBuilder
    {
        public static Weapon BuildWeapon(Weapon weapon) //Todo: should get an weaponID and initialize the weapon with these stats
        {
            //Todo: load stats from file
            float damage = 3f;
            float attackSpeed = 10f;
            float accuracy = 30f;
            float bulletSpeed = 20;
            float attackRange = 5f;

            weapon.Initialize(damage, attackSpeed, accuracy, bulletSpeed, attackRange);
            return weapon;
        }
    }
}