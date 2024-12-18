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
using System.Collections.Generic;

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

        private AudioManager audioManager;
        
        //particle system modules
        private ParticleSystem bulletSystem;
        private ParticleSystem.MainModule mainModule;
        private ParticleSystem.EmissionModule emissionModule;
        private ParticleSystem.ShapeModule shapeModule;

        public void Initialize(Dictionary<Stat, float> baseStats, Color bulletColor)
        {
            bulletSystem = GetComponentInChildren<ParticleSystem>();
            audioManager = FindObjectOfType<AudioManager>();
            emissionModule = bulletSystem.emission;
            mainModule = bulletSystem.main;
            shapeModule = bulletSystem.shape;
            
            //TODO move to seperate method
            this.damage = baseStats[Stat.BaseDamage];
            this.attackSpeed = baseStats[Stat.AttackSpeed];
            this.accuracy = baseStats[Stat.Accuracy];
            this.bulletSpeed = baseStats[Stat.BulletSpeed];
            AttackRange = baseStats[Stat.AttackRange];

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
            audioManager.Play("gun bearbeitet");
        }

        public void ReleaseTrigger()
        {
            if (emissionModule.rateOverTime.constant == 0) return;
            emissionModule.rateOverTime = 0;
        }
    }
}