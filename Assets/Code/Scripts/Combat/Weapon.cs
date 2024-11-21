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
using Audio;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        //stat values   
        private float damage;
        private float percentDamage;
        private float attackSpeed;
        public float AttackRange { get; private set; }

        //particle system modules
        private ParticleSystem bulletSystem;
        private ParticleSystem.EmissionModule emissionModule;

        public void Initialize(CharacterStats stats)
        {
            bulletSystem = GetComponentInChildren<ParticleSystem>();
            emissionModule = bulletSystem.emission;

            //set stats
            stats.onStatsChanged += UpdateDamage;
            stats.onStatsChanged += UpdateAttackSpeed;
            stats.onStatsChanged += UpdateAttackRange;

            percentDamage = stats.GetStat(Stat.PercentDamage);
            damage = stats.GetStat(Stat.BaseDamage);
            attackSpeed = stats.GetStat(Stat.AttackSpeed);
            AttackRange = stats.GetStat(Stat.AttackRange);
        }

        public float CalculateDamage(/*TODE: calculate with armor and resi*/)
        {
            return damage * percentDamage;
        }

        public void PullTrigger()
        {
            if (emissionModule.rateOverTime.constant != 0) return;
            emissionModule.rateOverTime = attackSpeed;
            bulletSystem.Play();
            FindObjectOfType<AudioManager>().Play("gun bearbeitet");
        }

        public void ReleaseTrigger()
        {
            if (emissionModule.rateOverTime.constant == 0) return;
            emissionModule.rateOverTime = 0;
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
            attackSpeed = newValue;
        }

        private void UpdateAttackRange(Stat stat, float newValue)
        {
            if (stat != Stat.AttackRange) return;
            AttackRange = newValue;

        }
    }
}