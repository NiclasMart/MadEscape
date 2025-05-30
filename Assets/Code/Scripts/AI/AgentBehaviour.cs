// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 16.04.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using CharacterProgressionMatrix;
using Generator;
using Stats;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class AgentBehaviour : MonoBehaviour
    {
        private const float PathUpdateThreshold = 1f;

        [SerializeField] private SkillTemplate UsedSkill;
        
        [SerializeField] protected bool DisableDistanceThresholdCheck = false;
        [SerializeField] protected bool PauseAgentDuringSkillUse = false;
        protected NavMeshAgent Agent { get; private set; }
        protected Room RoomRef { get; private set; }
        
        protected Skill Skill;
        private Func<bool> _skillCastAction;
        
        private Vector3 _targetPosition = Vector3.positiveInfinity;
        
        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            RoomRef = FindFirstObjectByType<Room>();

            if (UsedSkill != null)
            {
                Skill = Skill.CreateSkillFromTemplate(UsedSkill.info, gameObject);
            }
        }

        void Update()
        {
           bool SkillWasCasted = _skillCastAction?.Invoke() ?? false;
           if (PauseAgentDuringSkillUse && SkillWasCasted)
           {
               Agent.enabled = false;
               return;
           }
           Agent.enabled = true;
           
           // ensures, that the path is only updated, when the position exceeds a certain threshold
           Vector3 newTargetPosition = CalculateNewTargetPosition();
           if (!DisableDistanceThresholdCheck && (_targetPosition - newTargetPosition).sqrMagnitude < PathUpdateThreshold) return;
            
           // update target
           _targetPosition = newTargetPosition; 
           Agent.destination = newTargetPosition;
        }

        private void OnEnable()
        {
            _skillCastAction = Skill?.RegisterSkill();
        }

        private void OnDisable()
        {
            _skillCastAction = null;
            Skill.Disable();
        }

        public void Initialize(CharacterStats stats)
        {
            Agent.speed = stats.GetStat(Stat.MovementSpeed);
            Agent.stoppingDistance = stats.GetStat(Stat.AttackRange);
            
            Skill.Reset();
        }
        
        // this is true on game start and when the agent reached its destination
        protected bool HasReachedDestination()
        {
            return Agent.remainingDistance <= Agent.stoppingDistance && !Agent.pathPending;
        }
        
        protected abstract Vector3 CalculateNewTargetPosition();
    }
}