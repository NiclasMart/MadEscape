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
        private float spawnDelayTime;

        //particle system modules
        private ParticleSystem.EmissionModule emissionModule;

        public void Initialize(BaseController owner, CharacterStats stats)
        {
            emissionModule = bulletSystem.emission;
            
            //set stats
            stats.onStatsChanged += UpdateDamage;
            stats.onStatsChanged += UpdateAttackSpeed;
            percentDamage = stats.GetStat(Stat.PercentDamage);
            damage = stats.GetStat(Stat.BaseDamage);
            emissionModule.rateOverTime = 1f / stats.GetStat(Stat.AttackSpeed);

            SetFiringActiveState(true);
        }

        public float CalculateDamage(/*TODE: calculate with armor and resi*/)
        {
            return damage * percentDamage;
        }

        public void SetFiringActiveState(bool active)
        {
            emissionModule.enabled = active;
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