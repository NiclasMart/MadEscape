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
        public float AttackSpeed { get; private set; }
        public float AttackRange { get; private set; }

        private AudioManager audioManager;

        //particle system modules
        private ParticleSystem bulletSystem;
        private ParticleSystem.MainModule mainModule;
        private ParticleSystem.EmissionModule emissionModule;
        private ParticleSystem.ShapeModule shapeModule;
        private ParticleSystem.CollisionModule collisionModule;

        public void Initialize(Dictionary<Stat, float> baseStats, Color bulletColor, string targetLayer)
        {
            bulletSystem = GetComponentInChildren<ParticleSystem>();
            audioManager = FindFirstObjectByType<AudioManager>();
            emissionModule = bulletSystem.emission;
            mainModule = bulletSystem.main;
            shapeModule = bulletSystem.shape;
            collisionModule = bulletSystem.collision;

            //TODO move to seperate method
            damage = baseStats[Stat.BaseDamage];
            AttackSpeed = baseStats[Stat.AttackSpeed];
            AttackRange = baseStats[Stat.AttackRange];

            ParticleSystem.Burst burst = emissionModule.GetBurst(0);
            burst.count = baseStats[Stat.BulletCount];
            emissionModule.SetBurst(0, burst);

            mainModule.startSpeed = baseStats[Stat.BulletSpeed];
            mainModule.startLifetime = 50f / baseStats[Stat.BulletSpeed];
            shapeModule.angle = Mathf.Max(Mathf.Min(60f, -0.6f * baseStats[Stat.Accuracy] + 60f), 0); //100accuracy = 0angle, 0accuracy = 60angle
            mainModule.startColor = bulletColor;

            collisionModule.collidesWith = LayerMask.GetMask("Default", targetLayer);
        }

        public float CalculateDamage(/*TODE: calculate with armor and resi*/)
        {
            return damage;
        }

        public void Fire()
        {
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