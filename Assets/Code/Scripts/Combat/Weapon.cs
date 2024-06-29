// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 01.07.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Core;
using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(ObjectPool))]
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private EnemyFinder enemyFinder;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private Transform playerWeaponHolder;

        private ObjectPool projectilePool;

        private float damage;
        private float percentDamage;
        private float totalDamage;
        private float spawnDelayTime;
        private float timer = 0;

        private void Awake()
        {
            projectilePool = GetComponent<ObjectPool>();
        }

        public void Initialize(CharacterStats stats)
        {   
            stats.onStatsChanged += UpdateDamage;
            percentDamage = stats.GetStat(Stat.PercentDamage);
            damage = stats.GetStat(Stat.BaseDamage);
            totalDamage = damage * percentDamage;
            spawnDelayTime = 1f / stats.GetStat(Stat.AttackSpeed);
        }

        private void FixedUpdate()
        {
            if (timer > spawnDelayTime)
            {
                FireProjectile();
                timer = 0;
            }
            timer += Time.fixedDeltaTime;
        }

        private void Update()
        {
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
            Projectile projectile = projectilePool.GetObject().GetComponent<Projectile>();
            projectile.transform.position = projectileSpawnPoint.position;
            projectile.Fire(projectileSpawnPoint.transform.forward, totalDamage);
        }

        public void RotateTo(Vector3 position)
        {
            Vector3 targetPosition = new Vector3(position.x, playerWeaponHolder.transform.position.y, position.z);
            playerWeaponHolder.transform.LookAt(targetPosition);
        }

        private void UpdateDamage(Stat stat, float newValue)
        {   
            if(stat != Stat.BaseDamage && stat != Stat.PercentDamage) return;
            if(stat == Stat.BaseDamage)
            {
                Debug.Log($"Updated BaseDamage from {damage} to {newValue}");
                damage = newValue;
                totalDamage = damage * percentDamage;
            }

            if(stat == Stat.PercentDamage)
            {
                 Debug.Log($"Updated PercentDamage from {damage} to {newValue}");
                percentDamage = newValue;
                totalDamage = damage * percentDamage;
            }
            
        }
        
    }
}