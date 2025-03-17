// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 01.07.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Stats;
using UnityEngine;
using Audio;
using System.Collections.Generic;
using System;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        //stat values   
        private float _damage;
        public float AttackSpeed { get; private set; }
        public float AttackRange { get; private set; }

        private AudioManager _audioManager;

        //particle system modules
        private ParticleSystem _bulletSystem;
        private ParticleSystem.MainModule _mainModule;
        private ParticleSystem.EmissionModule _emissionModule;
        private ParticleSystem.ShapeModule _shapeModule;
        private ParticleSystem.CollisionModule _collisionModule;

        Dictionary<Stat, Action<float>> dict = new();

        public void Initialize(Dictionary<Stat, float> baseStats, Color bulletColor, string targetLayer, CharacterStats stats)
        {
            _bulletSystem = GetComponentInChildren<ParticleSystem>();
            _audioManager = FindFirstObjectByType<AudioManager>();
            _emissionModule = _bulletSystem.emission;
            _mainModule = _bulletSystem.main;
            _shapeModule = _bulletSystem.shape;
            _collisionModule = _bulletSystem.collision;

            //TODO move to seperate method
            _damage = baseStats[Stat.BaseDamage];
            AttackSpeed = baseStats[Stat.AttackSpeed];
            AttackRange = baseStats[Stat.AttackRange];

            ParticleSystem.Burst burst = _emissionModule.GetBurst(0);
            burst.count = baseStats[Stat.BulletCount];
            _emissionModule.SetBurst(0, burst);

            _mainModule.startSpeed = baseStats[Stat.BulletSpeed];
            _mainModule.startLifetime = 50f / baseStats[Stat.BulletSpeed];
            _shapeModule.angle = Mathf.Max(Mathf.Min(60f, -0.6f * baseStats[Stat.Accuracy] + 60f), 0); //100accuracy = 0angle, 0accuracy = 60angle
            _mainModule.startColor = bulletColor;

            _collisionModule.collidesWith = LayerMask.GetMask("Default", targetLayer);

            // Add stat update actions to the dictionary
            dict.Add(Stat.BaseDamage, (value) => { _damage = value; });
            dict.Add(Stat.AttackSpeed, (value) => { AttackSpeed = value; });
            dict.Add(Stat.AttackRange, (value) => { AttackRange = value; });
            dict.Add(Stat.BulletCount, (value) => {
                ParticleSystem.Burst updatedBurst = _emissionModule.GetBurst(0);
                updatedBurst.count = value;
                _emissionModule.SetBurst(0, updatedBurst);
                });
            dict.Add(Stat.BulletSpeed, (value) => {
                _mainModule.startSpeed = value;
                _mainModule.startLifetime = 50f / value;
                });
            dict.Add(Stat.Accuracy, (value) => {
                _shapeModule.angle = Mathf.Max(Mathf.Min(60f, -0.6f * value + 60f), 0);
                });
            // Subscribe to stat changes
            stats.onStatsChanged += UpdateStat;
        }

        public void UpdateStat(Stat stat, float value)
        {
            if (dict.ContainsKey(stat))
            {
                dict[stat].Invoke(value);
            }
        }

        // TODO: unsubscribe if weapon is destroyed? maybe if we change weapons

        public float CalculateDamage(/*TODE: calculate with armor and resi*/)
        {
            return _damage;
        }

        public void Fire()
        {
            _bulletSystem.Play();
            _audioManager.Play("gun bearbeitet");
        }

        public void ReleaseTrigger()
        {
            if (_emissionModule.rateOverTime.constant == 0) return;
            _emissionModule.rateOverTime = 0;
        }
    }
}