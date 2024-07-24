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
using UnityEngine.UI;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private EnemyFinder enemyFinder;
        [SerializeField] private ParticleSystem bullets;
        [SerializeField] private Transform playerWeaponHolder;

        //stat values   
        private float damage;
        private float percentDamage;
        private float spawnDelayTime;

        //particle system modules
        private ParticleSystem.EmissionModule emissionModule;

        public void Initialize(CharacterStats stats)
        {
            //get particle modules
            emissionModule = bullets.emission;

            stats.onStatsChanged += UpdateDamage;
            stats.onStatsChanged += UpdateAttackSpeed;

            percentDamage = stats.GetStat(Stat.PercentDamage);
            damage = stats.GetStat(Stat.BaseDamage);
            emissionModule.rateOverTime = 1f / stats.GetStat(Stat.AttackSpeed);

            SetFiringActiveState(true);
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

        public float CalculateDamage(/*TODE: calculate with armor and resi*/)
        {
            return damage * percentDamage;
        }

        public void SetFiringActiveState(bool active)
        {
            emissionModule.enabled = active;
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