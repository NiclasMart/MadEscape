// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 17.06.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyActions
{
    public class Move : Action
    {
        private Transform _target;

        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            if (_active && _agent)
            {
                _agent.destination = _target.position;
            }
        }

        public override void Initialize(CharacterStats stats)
        {
            base.Initialize(stats);
            _agent.speed = stats.GetStat(Stat.MovementSpeed);
            _agent.stoppingDistance = stats.GetStat(Stat.AttackRange);
        }

        public void SetTarget(Transform newTarget)
        {
            _target = newTarget;
        }
    }
}