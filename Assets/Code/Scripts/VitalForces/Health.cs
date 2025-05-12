// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 10.07.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using Combat;
using Stats;
using UnityEngine;
using System.Collections;

namespace VitalForces
{
    public class Health : Resource
    {
        private float lifeRegen;
        private float life;

        [SerializeField] public float HealthRestoreTime = 0.5f;

        private float armor;
        public bool IsAlive => CurrentValue > 0;

        public event Action<GameObject> OnDeath;
        public event Action<float> OnTakeDamage;

        public void Initialize(CharacterStats stats)
        {
            life = stats.GetStat(Stat.Life);
            lifeRegen = stats.GetStat(Stat.LifeRegen);
            armor = stats.GetStat(Stat.Armor);
            stats.OnStatsChanged += UpdateHealthStat;
            Initialize(life, life);
        }

        private void Update()
        {
            if (lifeRegen > 0)
            {
                RegenerateHealth(lifeRegen * Time.deltaTime);
            }
        }

        public void TakeDamage(float amount)
        {
            float damage = DamageCalculator.CalculateDamage(amount, armor);
            Change(-damage);
            OnTakeDamage?.Invoke(damage);
            if (!IsAlive) OnDeath?.Invoke(gameObject);
        }

        public void RegenerateHealth(float regenAmount)
        {
            Change(regenAmount);
        }
        public IEnumerator RestoreHealthOverTime(float totalAmount)
        {
            float elapsed = 0f;
            while (elapsed < HealthRestoreTime)
            {
                float delta = (totalAmount / HealthRestoreTime) * Time.deltaTime;
                RegenerateHealth(delta);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        public void UpdateHealthStat(Stat stat, float newValue)
        {
            if (stat != Stat.Life && stat != Stat.LifeRegen && stat != Stat.Armor) return;

            if (stat == Stat.Life)
            {
                life = newValue;
                UpdateDisplay(life);
            }
            if (stat == Stat.LifeRegen) lifeRegen = newValue;
            if (stat == Stat.Armor) armor = newValue;
        }
    }
}