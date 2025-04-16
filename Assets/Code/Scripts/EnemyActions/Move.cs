// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 17.06.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using AI;
using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyActions
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Move : Action
    {
        private Transform _target;
        private NavMeshAgent _agent;
        private AgentBehaviour _pathingBehaviour;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (_active)
            {
                _agent.destination = _pathingBehaviour.GetTargetPosition();
            }
        }

        public override void Initialize(CharacterStats stats)
        {
            base.Initialize(stats);
            _agent.speed = stats.GetStat(Stat.MovementSpeed);
            _agent.stoppingDistance = stats.GetStat(Stat.AttackRange);
        }

        public void ChangeBehaviour(AgentBehaviour behaviour)
        {
            _pathingBehaviour = behaviour;
        }
    }
}