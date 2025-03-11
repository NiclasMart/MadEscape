// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 10.07.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Combat;
using Stats;
using UI;
using UnityEngine;

namespace VitalForces
{
    public class Health : Resource
    {
        private float lifeRegen;
        private float life;

        private float armor;
        public bool isAlive => CurrentValue > 0;

        public Action<GameObject> OnDeath;
        public Action<float> OnTakeDamage;
        private float timer = 0f;

        public void Initialize(CharacterStats stats)
        {
            life = stats.GetStat(Stat.Life);
            lifeRegen = stats.GetStat(Stat.LifeRegen);
            armor = stats.GetStat(Stat.Armor);
            stats.onStatsChanged += UpdateHealthStat;
            Initialize(life, life);
        }

        private void Update()
        {
            if (timer > 1)
            {
                RegenerateHealth(lifeRegen);
                timer = 0f;
            }
            timer += Time.fixedDeltaTime;
        }

        public void TakeDamage(float amount)
        {
            float damage = DamageCalculator.CalculateDamage(amount, armor);
            Change(-damage);
            OnTakeDamage?.Invoke(damage);
            if (!isAlive) OnDeath(gameObject);
        }

        public void RegenerateHealth(float regenAmount)
        {
            Change(regenAmount);
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

        private void OnParticleCollision(GameObject other)
        {
            Weapon weapon = other.transform.parent.GetComponent<Weapon>();
            if (weapon != null)
            {
                float damage = weapon.CalculateDamage();
                TakeDamage(damage);
            }
        }
    }
}