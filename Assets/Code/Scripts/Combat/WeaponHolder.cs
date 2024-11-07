// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 24.07.24
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class WeaponHolder : MonoBehaviour
    {
        private EnemyFinderAll enemyFinderAll;
        private Weapon weapon;

        private void Awake()
        {
            enemyFinderAll = GetComponent<EnemyFinderAll>();
        }

        private void Update()
        {
            //handle weapon rotation
            Vector3 lookTargetPosition = transform.position + transform.forward;
            enemyFinderAll.GetClosestEnemy(out GameObject closestEnemy, out float distance);
            if (closestEnemy != null)
            {
                lookTargetPosition = closestEnemy.transform.position;
            }
            SetWeaponActiveState(distance <= weapon.AttackRange);
            RotateTo(lookTargetPosition);
        }

        public void SetWeaponActiveState(bool active)
        {
            if (!weapon) return;
            
            if (active) weapon.PullTrigger();
            else weapon.ReleaseTrigger();
        }

        public void SetWeapon(Weapon weapon)
        {
            this.weapon = weapon;
        }

        public void RotateTo(Vector3 position)
        {
            Vector3 targetPosition = new Vector3(position.x, transform.position.y, position.z);
            transform.LookAt(targetPosition);
        }
    }
}