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
        //stat values   
        private float damage;
        private float attackSpeed;
        private float accuracy;
        private float bulletSpeed;

        public float AttackRange { get; private set; }

        //particle system modules
        private ParticleSystem bulletSystem;
        private ParticleSystem.MainModule mainModule;
        private ParticleSystem.EmissionModule emissionModule;
        private ParticleSystem.ShapeModule shapeModule;

        public void Initialize(float damage, float attackSpeed, float accuracy, float bulletSpeed, float attackRange, Color bulletColor)
        {
            bulletSystem = GetComponentInChildren<ParticleSystem>();
            emissionModule = bulletSystem.emission;
            mainModule = bulletSystem.main;
            shapeModule = bulletSystem.shape;
            
            this.damage = damage;
            this.attackSpeed = attackSpeed;
            this.accuracy = accuracy;
            this.bulletSpeed = bulletSpeed;
            AttackRange = attackRange;

            mainModule.startSpeed = bulletSpeed;
            mainModule.startLifetime = 50f/bulletSpeed;
            mainModule.startColor = bulletColor;
            shapeModule.angle = Mathf.Max(Mathf.Min(60f,-0.6f*accuracy + 60f),0); //100accuracy = 0angle, 0accuracy = 60angle
        }

        public float CalculateDamage(/*TODE: calculate with armor and resi*/)
        {
            return damage;
        }

        public void PullTrigger()
        {
            if (emissionModule.rateOverTime.constant != 0) return;
            emissionModule.rateOverTime = attackSpeed;
            bulletSystem.Play();
        }

        public void ReleaseTrigger()
        {
            if (emissionModule.rateOverTime.constant == 0) return;
            emissionModule.rateOverTime = 0;
        }
    }
}