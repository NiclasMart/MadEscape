// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 17.06.23
// Author: salcintram07@web.de
// Origin Project: Sandbox_Project
// ---------------------------------------------
// -------------------------------------------*/

using EnemyActions;
using System.Collections.Generic;
using UnityEngine;
using Generation;
using Stats;
using Items;
using Audio;
using Combat;
using Core;
using AI;

namespace Controller
{
    public class EnemyController : BaseController, IPoolable
    {
        [HideInInspector] public Item _drop;
        
        //TODO: switch transform to find player in scene
        private Transform _target;
        private AgentBehaviour _activeAgentBehaviour;
        private Attack _baseAttack;

        public event System.Action<IPoolable> OnDestroy;

        protected override void Awake()
        {
            base.Awake();
            _baseAttack = GetComponent<Attack>();
            _activeAgentBehaviour = GetComponent<AgentBehaviour>();

        }

        protected override void OnEnable()
        {
            base.OnEnable(); // Calls BaseController's OnEnable to handle HandleDeath subscription

            // Subscribe to additional callbacks specific to EnemyController
            if (Health != null)
            {
                Health.OnTakeDamage += ServiceProvider.Get<StatisticTracker>().RegisterDealtDamage;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable(); // Calls BaseController's OnDisable to handle HandleDeath unsubscription

            // Unsubscribe from additional callbacks specific to EnemyController
            if (Health != null)
            {
                Health.OnTakeDamage -= ServiceProvider.Get<StatisticTracker>().RegisterDealtDamage;
            }
        }

        // This function sets up all important stat-related settings for the enemy and can be called multiple times (to change enemy type)
        public void Initialize(Dictionary<Stat, float> baseStats, Item drop)
        {
            base.Initialize(baseStats);

            FindPlayerTarget();

            Weapon weapon = MountWeapon(_target.gameObject, "Player");
            if (weapon) _stats.SetStat(Stat.AttackRange, weapon.AttackRange);

            _activeAgentBehaviour.Initialize(_stats);

            _baseAttack.Initialize(_stats);
            _baseAttack.Activate();

            _drop = drop; //Todo: don't tie loot to enemy, but to enemy spawner
        }

        private void FindPlayerTarget()
        {
            PlayerController player = FindFirstObjectByType<PlayerController>();
            if (player == null) Debug.LogError("Player not Found!");
            _target = player.transform;
        }

        protected override void HandleDeath(GameObject self)
        {
            //Todo: disable unused components
            DropLoot();
            ServiceProvider.Get<AudioManager>().Play(AudioActionType.Enemy_Death);

            ServiceProvider.Get<StatisticTracker>().RegisterKill();
            _stats.Clear();

            OnDestroy?.Invoke(this);
        }

        private void DropLoot()
        {
            if (_drop == null) return;

            GameObject pickup = new GameObject("ItemPickup");
            Pickup pickupRef = pickup.AddComponent<Pickup>();
            SphereCollider collider = pickup.AddComponent<SphereCollider>();
            pickup.layer = LayerMask.NameToLayer("EnemyProjectile");

            pickup.transform.position = transform.position;
            pickupRef.Generate(_drop);
            collider.radius = pickupRef.CollectRadius;
            collider.isTrigger = true;
        }

        public GameObject GetAttachedGameobject()
        {
            return gameObject;
        }

        public void Reset()
        {
            //Todo: handle reset of all classes that get initialized on new spawn
        }
    }
}