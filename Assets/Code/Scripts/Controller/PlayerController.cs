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
using VitalForces;
using System;
using Stats;
using Items;
using Core;

namespace Controller
{
    public class PlayerController : BaseController
    {


        private PlayerMover _mover;
        private ItemInventory _inventory;
        public ItemInventory Inventory => _inventory;
        private Sanity _sanity;

        public Action OnDeath;

        protected override void Awake()
        {
            base.Awake();
            _mover = GetComponent<PlayerMover>();
            _sanity = GetComponent<Sanity>();
            _inventory = GetComponentInChildren<ItemInventory>();
        }

        private void Start()
        {
            LoadCharacterStats();

            _mover.Initialize(_stats);
            _sanity.Initialize(_stats); //TODO: connect SanityDecSpeed
            _inventory.Initialize(gameObject);
            _health.OnTakeDamage += StatisticTracker.Instance.RegisterSufferedDamage;

            string targetLayer = "Enemy";
            MountWeapon(null, targetLayer);
        }

        private void LoadCharacterStats()
        {
            //TODO: decide if we want to store all other loaded character stats OR only load the defined ID stats
            List<StatRecord> loadedStatData = LoadStats.LoadPlayerStats();
            Dictionary<Stat, float> baseStats;
            if (loadedStatData.Count - 1 < _statID)
            {
                baseStats = loadedStatData[0].statDict;
                Debug.LogWarning("For the set playerID no data is availabe. Loaded default player insted.");
            }
            else
            {
                baseStats = loadedStatData[_statID].statDict;
            }
            base.Initialize(baseStats);
        }

        protected override void HandleDeath(GameObject self)
        {
            OnDeath();
            _health.OnTakeDamage -= StatisticTracker.Instance.RegisterSufferedDamage;
        }

        //this is just for reference to see how it could be done later
        public void SwitchPlayer(int playerId)
        {
            StatRecord playerData = LoadStats.LoadPlayerStats()[playerId];
            _stats.SetNewStatData(playerData.statDict);
        }
    }
}