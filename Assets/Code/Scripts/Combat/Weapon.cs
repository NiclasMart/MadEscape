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

        public void Initialize(Dictionary<Stat, float> baseStats, Color bulletColor, string targetLayer, Action<Stat, float> onStatChange)
        {
            _bulletSystem = GetComponentInChildren<ParticleSystem>();
            _audioManager = ServiceProvider.Get<AudioManager>();

            _emissionModule = _bulletSystem.emission;
            _mainModule = _bulletSystem.main;
            _shapeModule = _bulletSystem.shape;
            _collisionModule = _bulletSystem.collision;

            CreateStatUpdateDictionary();

            foreach (var statRecord in baseStats)
            {
                UpdateStat(statRecord.Key, statRecord.Value);
                onStatChange?.Invoke(statRecord.Key, statRecord.Value);
            }

            _mainModule.startColor = bulletColor;
            _collisionModule.collidesWith = LayerMask.GetMask("Default", targetLayer);
        }


        //Don't use directly, use the StatUpdate methode from the WeaponController instead
        public void UpdateStat(Stat stat, float value)
        {
            if (dict.ContainsKey(stat))
            {
                dict[stat]?.Invoke(value);
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
            _audioManager.Play(AudioActionType.WeaponShoot_Pistol, Priority.High);
        }

        public void ReleaseTrigger()
        {
            if (_emissionModule.rateOverTime.constant == 0) return;
            _emissionModule.rateOverTime = 0;
        }

        private void CreateStatUpdateDictionary()
        {
            // Clear the dictionary to avoid duplicate keys
            dict.Clear();

            dict.Add(Stat.BaseDamage, (value) => { _damage = value; });
            dict.Add(Stat.AttackSpeed, (value) => { AttackSpeed = value; });
            dict.Add(Stat.AttackRange, (value) => { AttackRange = value; });
            dict.Add(Stat.BulletCount, (value) =>
            {
                ParticleSystem.Burst updatedBurst = _emissionModule.GetBurst(0);
                updatedBurst.count = value;
                _emissionModule.SetBurst(0, updatedBurst);
            });
            dict.Add(Stat.BulletSpeed, (value) =>
            {
                _mainModule.startSpeed = value;
                _mainModule.startLifetime = 50f / value;
            });
            dict.Add(Stat.Accuracy, (value) =>
            {
                _shapeModule.angle = Mathf.Max(Mathf.Min(60f, -0.6f * value + 60f), 0); // 100 accuracy = 0 angle, 0 accuracy = 60 angle
            });
        }
    }
}