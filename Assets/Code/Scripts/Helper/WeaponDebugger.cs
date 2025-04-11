// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 11.04.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Stats;
using UnityEngine;

namespace Helper
{
    public class WeaponDebugger : MonoBehaviour
    {
        [SerializeField] private WeaponTemplate _startWeapon;
        private Weapon _weapon;
        private float _timeSinceLastShot;

        public Action<Stat, float> OnStatChanged;

        void Awake()
        {
            _weapon = Instantiate(_startWeapon.WeaponModel).GetComponent<Weapon>();
            var statDict = WeaponBuilder.GetWeaponStats(_startWeapon.WeaponID);
            _weapon.Initialize(statDict, _startWeapon.BulletColor, "Enemy", OnStatChanged);
        }

        void Update()
        {
            if (_timeSinceLastShot > 1 / _weapon.AttackSpeed)
            {
                _weapon.Fire();
                _timeSinceLastShot = 0;
            }
            _timeSinceLastShot += Time.deltaTime;
        }

    }
}