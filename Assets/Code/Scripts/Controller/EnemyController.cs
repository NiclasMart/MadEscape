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

//TODO: Check if another partent class (Controller) should be created, from which this class and a potential PlayerController can inherit
namespace Controller
{
    public class EnemyController : BaseController, IPoolable
    {
        //TODO: switch transform to find player in scene
        private Transform _target;
        private Move _mover;
        private Attack _attack;
        public List<Item> _loot = new List<Item>();

        public event System.Action<IPoolable> OnDestroy;

        protected override void Awake()
        {
            base.Awake();
            _mover = GetComponent<Move>();
            _attack = GetComponent<Attack>();
        }

        //this function sets up all important stat related settings for the enemy and can be called multiple times (to change enemy type)
        public void Initialize(Dictionary<Stat, float> baseStats, List<Item> loot)
        {
            base.Initialize(baseStats);

            FindPlayerTarget();

            string targetLayer = "Player";
            Weapon weapon = MountWeapon(_target.gameObject, targetLayer);
            if (weapon) _stats.SetStat(Stat.AttackRange, weapon.AttackRange);

            _mover.Initialize(_stats);
            _mover.Activate();
            _mover.SetTarget(_target);

            _attack.Initialize(_stats);
            _attack.Activate();

            this._loot = loot; //Todo: don't tie loot to enemy, but to enemy spawner
        }

        public GameObject GetAttachedGameobject()
        {
            return gameObject;
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
            _stats.Clear();
            DropLoot();
            OnDestroy(this);
            _health.onDeath = null;
            FindFirstObjectByType<AudioManager>().Play("enemy death");

        }

        private void DropLoot()
        {
            float dropChance = 0.05f;
            float randomFloat = Random.Range(0f, 1f);

            if (randomFloat <= dropChance)
            {
                Item item = _loot[Random.Range(0, _loot.Count)];
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
}