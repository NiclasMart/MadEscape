// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 17.06.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

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
    public class PlayerController : BaseController
    {
        [SerializeField] private int playerID = 0;
        [SerializeField] private Weapon weapon;

        private PlayerMover mover;
        private EnemyFinder enemyFinder;

        private ItemInventory inventory;
        public ItemInventory Inventory => inventory;

        public Action onDeath;
        private Sanity sanity;

        protected override void Awake()
        {
            base.Awake();
            mover = GetComponent<PlayerMover>();
            enemyFinder = GetComponentInChildren<EnemyFinder>();
            sanity = GetComponent<Sanity>();
            inventory = GetComponentInChildren<ItemInventory>();
        }

        private void Start()
        {
            //TODO: decide if we want to store all other loaded character stats OR only load the defined ID stats
            List<StatRecord> loadedStatData = LoadStats.LoadPlayerStats();
            Dictionary<Stat, float> baseStats;
            if (loadedStatData.Count - 1 < playerID)
            {
                baseStats = loadedStatData[0].statDict;
                Debug.LogWarning("For the set playerID no data is availabe. Loaded default player insted.");
            }
            else
            {
                baseStats = loadedStatData[playerID].statDict;
            }

            base.Initialize(baseStats);

            mover.Initialize(stats);
            enemyFinder.Initialize(stats);
            sanity.Initialize(stats); //TODO: connect SanityDecSpeed
            weapon.Initialize(stats);
            inventory.Initialize(gameObject);
        }

        protected override void HandleDeath(GameObject self)
        {
            onDeath();
        }

        //this is just for reference to see how it could be done later
        public void SwitchPlayer(int playerId)
        {
            StatRecord playerData = LoadStats.LoadPlayerStats()[playerId];
            stats.SetNewStatData(playerData.statDict);
        }
    }
}