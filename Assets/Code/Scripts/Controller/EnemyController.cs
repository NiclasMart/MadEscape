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
using VitalForces;
using Generation;
using Stats;
using Items;

//TODO: Check if another partent class (Controller) should be created, from which this class and a potential PlayerController can inherit
namespace Controller
{
    public class EnemyController : MonoBehaviour, IPoolable
    {
        //TODO: switch transform to find player in scene
        private Transform target;
        private Move mover;
        private Attack attack;
        private Health health;
        private CharacterStats stats;
        public List<Item> loot = new List<Item>();

        public event System.Action<IPoolable> onDestroy;

        private void Awake()
        {
            stats = GetComponent<CharacterStats>();
            mover = GetComponent<Move>();
            attack = GetComponent<Attack>();
            health = GetComponent<Health>();
        }

        //this function sets up all important stat related settings for the enemy and can be called multiple times (to change enemy type)
        public void Initialize(BaseStats baseStats, List<Item> loot)
        {
            FindPlayerTarget();

            stats.SetNewStatData(baseStats);

            health.Initialize(stats, HandleDeath);
            mover.Initialize(stats);
            mover.Activate();
            mover.SetTarget(target);

            attack.Initialize(stats);
            attack.Activate();

            this.loot = loot;
        }

        public GameObject GetAttachedGameobject()
        {
            return gameObject;
        }

        private void FindPlayerTarget()
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player == null) Debug.LogError("Player not Found!");
            target = player.transform;
        }

        private void HandleDeath()
        {
            //Todo: disable unused components
            stats.Clear();
            DropLoot();
            onDestroy(this);
        }

        private void SwitchAction(Action currentAction, Action newAction)
        {
            currentAction.Deactivate();
            newAction.Activate();
        }

        private void DropLoot()
        {
            Item item = loot[Random.Range(0, loot.Count)];
            GameObject pickup = new GameObject("ItemPickup");
            Pickup pickupRef = pickup.AddComponent<Pickup>();
            SphereCollider collider = pickup.AddComponent<SphereCollider>();
            pickup.layer = LayerMask.NameToLayer("EnemyProjectile");

            pickup.transform.position = transform.position;
            pickupRef.Generate(item);
            collider.radius = pickupRef.collectRadius;
            collider.isTrigger = true;
        }
    }
}