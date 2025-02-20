// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 20.11.24
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/WeaponsTemplate")]
    public class WeaponTemplate : ScriptableObject
    {
        public float WeaponID;
        public Color BulletColor;
        public GameObject WeaponModel;
    }
}