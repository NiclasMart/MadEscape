// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 01/07/23
// Author: langner.paul@t-online.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Controller;
using VitalForces;
using Stats;
using UnityEngine;

namespace EnemyActions
{
    public class Attack : Action
    {
        private float _attackDamage;
        private float _attackInterval;
        private float _nextAttackTime;
        private bool _attacked;

        public override void Initialize(CharacterStats stats)
        {
            base.Initialize(stats);
            _attackDamage = stats.GetStat(Stat.BaseDamage);
            _attackInterval = 1f / stats.GetStat(Stat.AttackSpeed);
            _nextAttackTime = Time.time;
            _attacked = false;
        }

        private void OnTriggerEnter(Collider colliderInfo)
        {
            if (colliderInfo.GetComponent<PlayerController>() != null)
                PerformAttack(colliderInfo.gameObject);
        }

        private void OnTriggerStay(Collider colliderInfo)
        {
            if (colliderInfo.GetComponent<PlayerController>() != null)
                PerformAttack(colliderInfo.gameObject);
        }

        private void PerformAttack(GameObject target)
        {
            if (_nextAttackTime <= Time.time)
            {
                _attacked = false;
            }

            if (!_attacked && _active)
            {
                if (target.GetComponent<PlayerController>())
                {
                    //TODO: maybe we can add a function to get the health directly via the player controller, so we only need one GetComponent call
                    Health targetHealth = target.GetComponent<Health>();
                    targetHealth.ApplyDamage(_attackDamage);
                    _nextAttackTime = Time.time + _attackInterval;
                    _attacked = true;
                }
            }
        }
    }
}