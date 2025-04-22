// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 24.07.24
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine;
using VitalForces;
using Stats;
using System;

namespace Combat
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform _weaponAttacheTransform;
        [SerializeField] private Sanity sanity;
        private EnemyFinderAll _enemyFinderAll;
        private Weapon _weapon;
        private float _timeSinceLastShot;
        private float sanityFactor;

        public event Action<Stat, float> OnStatChanged;

        const float SANITY_ATTACKSPEED_FACTOR = 2f;

        private void Awake()
        {
            _enemyFinderAll = GetComponent<EnemyFinderAll>();
            if (sanity == null)
            {
                sanity = FindFirstObjectByType<Sanity>();
            }
        }

        public Weapon InitWeapon(WeaponTemplate weaponConfig, string targetLayer)
        {
            if (_weapon == null) _weapon = Instantiate(weaponConfig.WeaponModel, _weaponAttacheTransform).GetComponentInChildren<Weapon>();

            int weaponID = weaponConfig.WeaponID;
            var statDict = WeaponBuilder.GetWeaponStats(weaponID);

            _weapon.Initialize(weaponConfig.BulletColor, weaponConfig.ShootSFX, targetLayer);

            // set all weapon stats
            foreach (var stat in statDict.Keys)
            {
                UpdateStat(stat, statDict[stat]);
            }
            
            return _weapon;
        }

        private void Update()
        {
            _enemyFinderAll.GetClosestEnemy(out GameObject closestEnemy, out float distance);
            RotateToEnemy(closestEnemy);
            if (distance <= _weapon.AttackRange) FireWeapon();
        }

        public void UpdateStat(Stat stat, float value)
        {
            _weapon.UpdateStat(stat, value);
            OnStatChanged?.Invoke(stat, value);
        }

        public void SetTarget(GameObject target)
        {
            _enemyFinderAll.Initialize(target);
        }

        private void RotateTo(Vector3 position)
        {
            Vector3 targetPosition = new Vector3(position.x, transform.position.y, position.z);
            transform.LookAt(targetPosition);
        }

        private void RotateToEnemy(GameObject enemy)
        {
            Vector3 lookTargetPosition;
            if (enemy != null)
            {
                lookTargetPosition = enemy.transform.position;
            }
            else
            {
                lookTargetPosition = transform.position + transform.forward;
            }
            RotateTo(lookTargetPosition);
        }

        private void FireWeapon()
        {
            sanityFactor = sanity.CurrentValue / sanity.MaxValue; // Wert zwischen 0 und 1
            if (_timeSinceLastShot > 1 / (_weapon.AttackSpeed * (SANITY_ATTACKSPEED_FACTOR - sanityFactor)))
            {
                _weapon.Fire();
                _timeSinceLastShot = 0;
            }
            _timeSinceLastShot += Time.deltaTime;
        }
    }
}