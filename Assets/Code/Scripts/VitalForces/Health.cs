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
using Stats;
using UnityEngine;

namespace VitalForces
{
    public class Health : Resource
    {
        private float lifeRegen;
        private float life;
        public bool isAlive => CurrentValue > 0;

        public Action onDeath;
        private float timer = 0f;

        public void Initialize(CharacterStats stats, Action onDeath)
        {
            life = stats.GetStat(Stat.Life);
            lifeRegen = stats.GetStat(Stat.LifeRegen);
            stats.onStatsChanged += UpdateHealthStat;
            Initialize(life, life);
            this.onDeath = onDeath;
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

        //returns if damage is lethal
        public bool TakeDamage(float amount)
        {
            Change(-amount);
            if (!isAlive) onDeath();

            return !isAlive;
        }

        public void RegenerateHealth(float regenAmount)
        {
            Change(regenAmount);
        }

        public void UpdateHealthStat(Stat stat, float newValue)
        {
            if (stat != Stat.Life && stat != Stat.LifeRegen) return;
            Debug.Log($"Updated MaxHealth from {life} to {newValue}");

            if (stat == Stat.Life) life = newValue; UpdateDisplay(life);
            if (stat == Stat.LifeRegen) lifeRegen = newValue;

        }
    }
}