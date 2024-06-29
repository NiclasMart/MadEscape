// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 17.06.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;
using Combat;
using VitalForces;
using System;
using Stats;
using Items;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        private CharacterStats stats;

        private PlayerMover mover;
        private Health health;
        private EnemyFinder enemyFinder;

        private ItemInventory inventory;
        public ItemInventory Inventory => inventory;

        public Action onDeath;
        private Sanity sanity;

        private void Awake()
        {
            stats = GetComponent<CharacterStats>();

            mover = GetComponent<PlayerMover>();
            health = GetComponent<Health>();
            enemyFinder = GetComponentInChildren<EnemyFinder>();
            sanity = GetComponent<Sanity>();
            inventory = GetComponentInChildren<ItemInventory>();
        }

        private void Start()
        {
            mover.Initialize(stats);
            health.Initialize(stats, HandleDeath);
            enemyFinder.Initialize(stats);
            sanity.Initialize(stats); //TODO: connect SanityDecSpeed
            weapon.Initialize(stats);
            inventory.Initialize(gameObject);
        }

        private void HandleDeath()
        {
            onDeath();
        }
    }
}