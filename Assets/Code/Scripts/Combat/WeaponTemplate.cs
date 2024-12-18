// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 20.11.24
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/WeaponsTemplate")]
    public class WeaponTemplate : ScriptableObject
    {
        public float weaponID;
        public Color bulletColor;
        public GameObject weaponModel;
    }
}