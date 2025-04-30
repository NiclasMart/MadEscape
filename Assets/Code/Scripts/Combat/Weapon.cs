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
using VitalForces;
using UnityEngine.Purchasing;

namespace Combat
{
    public class Weapon : MonoBehaviour, ISocketable
    {
        //stat values   
        private float _damage;
        public float AttackSpeed { get; private set; }
        public float AttackRange { get; private set; }

        private AudioManager _audioManager;
        private AudioActionType _shootSFX;

        //particle system modules
        private ParticleSystem _bulletSystem;
        private ParticleSystem.MainModule _mainModule;
        private ParticleSystem.EmissionModule _emissionModule;
        private ParticleSystem.ShapeModule _shapeModule;
        private ParticleSystem.CollisionModule _collisionModule;

        Dictionary<Stat, Action<float>> statChangeCallbackDictionary = new();

        public void Initialize(Color bulletColor, AudioActionType shootSoundType, string targetLayer)
        {
            _audioManager = ServiceProvider.Get<AudioManager>();
            _shootSFX = shootSoundType;

            _bulletSystem = GetComponentInChildren<ParticleSystem>();
            _emissionModule = _bulletSystem.emission;
            _mainModule = _bulletSystem.main;
            _shapeModule = _bulletSystem.shape;
            _collisionModule = _bulletSystem.collision;

            CreateStatUpdateDictionary();

            _mainModule.startColor = bulletColor;
            _collisionModule.collidesWith = LayerMask.GetMask("Default", targetLayer);
        }


        //Don't use directly, use the StatUpdate methode from the WeaponController instead
        public void UpdateStat(Stat stat, float value)
        {
            if (statChangeCallbackDictionary.ContainsKey(stat))
            {
                statChangeCallbackDictionary[stat]?.Invoke(value);
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
            _audioManager.Play(_shootSFX);
        }

        public void ReleaseTrigger()
        {
            if (_emissionModule.rateOverTime.constant == 0) return;
            _emissionModule.rateOverTime = 0;
        }

        private void CreateStatUpdateDictionary()
        {
            // Clear the dictionary to avoid duplicate keys
            statChangeCallbackDictionary.Clear();

            statChangeCallbackDictionary.Add(Stat.BaseDamage, (value) => { _damage = value; });
            statChangeCallbackDictionary.Add(Stat.AttackSpeed, (value) => { AttackSpeed = value; });
            statChangeCallbackDictionary.Add(Stat.AttackRange, (value) => { AttackRange = value; });
            statChangeCallbackDictionary.Add(Stat.BulletCount, (value) =>
            {
                ParticleSystem.Burst updatedBurst = _emissionModule.GetBurst(0);
                updatedBurst.count = value;
                _emissionModule.SetBurst(0, updatedBurst);
            });
            statChangeCallbackDictionary.Add(Stat.BulletSpeed, (value) =>
            {
                _mainModule.startSpeed = value;
                _mainModule.startLifetime = 50f / value;
            });
            statChangeCallbackDictionary.Add(Stat.Accuracy, (value) =>
            {
                _shapeModule.angle = Mathf.Max(Mathf.Min(60f, -0.6f * value + 60f), 0); // 100 accuracy = 0 angle, 0 accuracy = 60 angle
            });
        }

        private List<ParticleCollisionEvent> _collisionEvents = new();
        private void OnParticleCollision(GameObject hitObject)
        {
            if (hitObject.TryGetComponent<Health>(out Health health))
            {
                int amount = _bulletSystem.GetCollisionEvents(hitObject, _collisionEvents);
                float damage = amount * CalculateDamage();
                health.TakeDamage(damage);

                Debug.Log($"BulletHits: {amount} on {hitObject.name} with {damage} damage");
            }
        }
    }
}