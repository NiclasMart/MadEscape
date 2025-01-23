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
    public class WeaponController : MonoBehaviour
    {
        private EnemyFinderAll enemyFinderAll;
        private Weapon weapon;
        private float timeSinceLastShot;

        private void Awake()
        {
            enemyFinderAll = GetComponent<EnemyFinderAll>();
            weapon = GetComponentInChildren<Weapon>();
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
            if (distance <= weapon.AttackRange) FireWeapon();
            RotateTo(lookTargetPosition);
        }

        public void SetTarget(GameObject target)
        {
            enemyFinderAll.Initialize(target);
        }

        public void FireWeapon()
        {
            if (timeSinceLastShot > 1 / weapon.AttackSpeed)
            {
                weapon.Fire();
                timeSinceLastShot = 0;
            }
            timeSinceLastShot += Time.deltaTime;
        }

        public Weapon InitWeapon(WeaponTemplate weaponData, string targetLayer)
        {
            if (weapon == null) weapon = Instantiate(weaponData.weaponModel, transform).GetComponentInChildren<Weapon>();
            WeaponBuilder.ConfigureWeapon(weapon, weaponData, targetLayer);
            return weapon;
        }

        public void RotateTo(Vector3 position)
        {
            Vector3 targetPosition = new Vector3(position.x, transform.position.y, position.z);
            transform.LookAt(targetPosition);
        }
    }
}