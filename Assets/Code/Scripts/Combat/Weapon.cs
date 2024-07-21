// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 01.07.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Core;
using Stats;
using UnityEngine;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private EnemyFinder enemyFinder;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private Transform playerWeaponHolder;

        private float damage;
        private float percentDamage;
        private float spawnDelayTime;
        private float timer = 0;

        private void Awake()
        {
            //TODO: initialize Particle System
        }

        public void Initialize(CharacterStats stats)
        {
            stats.onStatsChanged += UpdateDamage;
            stats.onStatsChanged += UpdateAttackSpeed;
            percentDamage = stats.GetStat(Stat.PercentDamage);
            damage = stats.GetStat(Stat.BaseDamage);
            spawnDelayTime = 1f / stats.GetStat(Stat.AttackSpeed);
        }

        private void FixedUpdate()
        {

        }

        private void Update()
        {
            //handle weapon rotaiton
            Vector3 lookTargetPosition = transform.position + transform.forward;
            GameObject closestEnemy = enemyFinder.GetClosestEnemy();
            if (closestEnemy != null)
            {
                lookTargetPosition = closestEnemy.transform.position;
            }
            RotateTo(lookTargetPosition);
        }

        private void FireProjectile()
        {
            // Projectile projectile = projectilePool.GetObject().GetComponent<Projectile>();
            // projectile.transform.position = projectileSpawnPoint.position;
            // float totalDamage = damage * percentDamage;
            // projectile.Fire(projectileSpawnPoint.transform.forward, totalDamage);
        }

        public void RotateTo(Vector3 position)
        {
            Vector3 targetPosition = new Vector3(position.x, playerWeaponHolder.transform.position.y, position.z);
            playerWeaponHolder.transform.LookAt(targetPosition);
        }

        private void UpdateDamage(Stat stat, float newValue)
        {
            if (stat != Stat.BaseDamage && stat != Stat.PercentDamage) return;
            if (stat == Stat.BaseDamage) damage = newValue;
            if (stat == Stat.PercentDamage) percentDamage = newValue;
        }

        private void UpdateAttackSpeed(Stat stat, float newValue)
        {
            if (stat != Stat.AttackSpeed) return;
            {
                spawnDelayTime = 1f / newValue;
            }
        }

    }
}