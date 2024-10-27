// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 01.07.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Controller;
using Stats;
using UnityEngine;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private ParticleSystem bulletSystem;

        //stat values   
        private float damage;
        private float percentDamage;
        public float AttackRange { get; private set; }
        private float spawnDelayTime;

        //particle system modules
        private ParticleSystem.EmissionModule emissionModule;

        public void Initialize(BaseController owner, CharacterStats stats)
        {
            emissionModule = bulletSystem.emission;
            
            //set stats
            stats.onStatsChanged += UpdateDamage;
            stats.onStatsChanged += UpdateAttackSpeed;
            stats.onStatsChanged += UpdateAttackRange;
            percentDamage = stats.GetStat(Stat.PercentDamage);
            damage = stats.GetStat(Stat.BaseDamage);
            emissionModule.rateOverTime = 1f / stats.GetStat(Stat.AttackSpeed);
            AttackRange = stats.GetStat(Stat.AttackRange);
        }

        public float CalculateDamage(/*TODE: calculate with armor and resi*/)
        {
            return damage * percentDamage;
        }

        public void PullTrigger()
        {
            emissionModule.enabled = true;
        }

        public void ReleaseTrigger()
        {
            emissionModule.enabled = false;
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
            spawnDelayTime = 1f / newValue;
        }

        private void UpdateAttackRange(Stat stat, float newValue)
        {
            if (stat != Stat.AttackRange) return;
            AttackRange = newValue;

        }
    }
}