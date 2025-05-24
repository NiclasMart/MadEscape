// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 16.04.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Generator;
using Stats;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class AgentBehaviour : MonoBehaviour
    {
        private const float PathUpdateThreshold = 1f;
        
        protected bool DisableThresholdCheck = false;
        protected NavMeshAgent Agent { get; private set; }
        protected Room RoomRef { get; private set; }
        
        private Vector3 _targetPosition = Vector3.positiveInfinity;
        
        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            RoomRef = FindFirstObjectByType<Room>();
        }

        void Update()
        {
            // ensures, that the path is only updated, when the position exceeds a certain threshold
            Vector3 newTargetPosition = CalculateNewTargetPosition();
            if (!DisableThresholdCheck && (_targetPosition - newTargetPosition).sqrMagnitude < PathUpdateThreshold) return;
            
            _targetPosition = newTargetPosition;
            Agent.destination = newTargetPosition;
        }
        
        public void Initialize(CharacterStats stats)
        {
            Agent.speed = stats.GetStat(Stat.MovementSpeed);
            Agent.stoppingDistance = stats.GetStat(Stat.AttackRange);
        }
        
        // this is true on game start and when the agent reached its destination
        protected bool HasReachedDestination()
        {
            return Agent.remainingDistance <= Agent.stoppingDistance && !Agent.pathPending;
        }
        
        protected abstract Vector3 CalculateNewTargetPosition();
    }
}