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
        private float attackDamage;
        private float attackInterval;
        private float nextAttackTime;
        private bool attacked;

        public override void Initialize(CharacterStats stats)
        {
            base.Initialize(stats);
            attackDamage = stats.GetStat(Stat.BaseDamage);
            attackInterval = 1f / stats.GetStat(Stat.AttackSpeed);
            nextAttackTime = Time.time;
            attacked = false;
        }

        private void OnCollisionEnter(Collision collisionInfo)
        {
            PerformAttack(collisionInfo.gameObject);
        }

        private void OnCollisionStay(Collision collisionInfo)
        {
            PerformAttack(collisionInfo.gameObject);
        }

        private void PerformAttack(GameObject target)
        {
            if (nextAttackTime <= Time.time)
            {
                attacked = false;
            }

            if (!attacked && active)
            {
                if (target.GetComponent<PlayerController>())
                {
                    //TODO: maybe we can add a function to get the health directly via the player controller, so we only need one GetComponent call
                    Health targetHealth = target.GetComponent<Health>();
                    targetHealth.TakeDamage(attackDamage);
                    nextAttackTime = Time.time + attackInterval;
                    attacked = true;
                }
            }
        }
    }
}