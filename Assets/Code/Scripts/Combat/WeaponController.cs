// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 24.07.24
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine;
using VitalForces;

namespace Combat
{
    public class WeaponController : MonoBehaviour
    {
        private EnemyFinderAll _enemyFinderAll;
        private Weapon _weapon;
        private float _timeSinceLastShot;

        private float sanityFactor;
        const float SANITY_ATTACKSPEED_FACTOR = 2f;
        [SerializeField] private Sanity sanity;

        [System.Obsolete]
        private void Awake()
        {
            _enemyFinderAll = GetComponent<EnemyFinderAll>();
            _weapon = GetComponentInChildren<Weapon>();
            if (sanity == null)
            {
                sanity = FindFirstObjectByType<Sanity>(); // Sucht automatisch nach einer vorhandenen Sanity-Komponente
            }
        }

        private void Update()
        {
            //handle weapon rotation
            Vector3 lookTargetPosition = transform.position + transform.forward;
            _enemyFinderAll.GetClosestEnemy(out GameObject closestEnemy, out float distance);
            if (closestEnemy != null)
            {
                lookTargetPosition = closestEnemy.transform.position;
            }
            if (distance <= _weapon.AttackRange) FireWeapon();
            RotateTo(lookTargetPosition);
        }

        public void SetTarget(GameObject target)
        {
            _enemyFinderAll.Initialize(target);
        }

        public void FireWeapon()
        {
            sanityFactor = sanity.CurrentValue / sanity.MaxValue; // Wert zwischen 0 und 1
            if (_timeSinceLastShot > 1 / (_weapon.AttackSpeed * (SANITY_ATTACKSPEED_FACTOR - sanityFactor)))
            {
                _weapon.Fire();
                _timeSinceLastShot = 0;
            }
            _timeSinceLastShot += Time.deltaTime;
        }

        public Weapon InitWeapon(WeaponTemplate weaponData, string targetLayer)
        {
            if (_weapon == null) _weapon = Instantiate(weaponData.WeaponModel, transform).GetComponentInChildren<Weapon>();
            WeaponBuilder.ConfigureWeapon(_weapon, weaponData, targetLayer);
            return _weapon;
        }

        public void RotateTo(Vector3 position)
        {
            Vector3 targetPosition = new Vector3(position.x, transform.position.y, position.z);
            transform.LookAt(targetPosition);
        }
    }
}