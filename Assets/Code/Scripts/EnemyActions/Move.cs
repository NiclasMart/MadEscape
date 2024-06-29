// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 17.06.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Data;
using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyActions
{
    public class Move : Action
    {
        private Transform target;

        private NavMeshAgent agent;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            if (active && agent)
            {
                agent.destination = target.position;
            }
        }

        public override void Initialize(CharacterStats stats)
        {
            base.Initialize(stats);
            agent.speed = stats.GetStat(Stat.MovementSpeed);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}