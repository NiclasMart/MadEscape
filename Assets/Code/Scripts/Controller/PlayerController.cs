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

        protected override void OnEnable()
        {
            base.OnEnable(); // Calls BaseController's OnEnable to handle HandleDeath subscription

            // Subscribe to additional callbacks specific to PlayerController
            if (_health != null)
            {
                _health.OnTakeDamage += ServiceProvider.Get<StatisticTracker>()?.RegisterSufferedDamage;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable(); // Calls BaseController's OnDisable to handle HandleDeath unsubscription

            // Unsubscribe from additional callbacks specific to PlayerController
            if (_health != null)
            {
                _health.OnTakeDamage -= ServiceProvider.Get<StatisticTracker>()?.RegisterSufferedDamage;
            }
        }

        private void Start()
        {
            LoadCharacterStats();

            _mover.Initialize(_stats);
            _sanity.Initialize(_stats); //TODO: connect SanityDecSpeed
            _inventory.Initialize(gameObject);

            string targetLayer = "Enemy";
            MountWeapon(null, targetLayer);
        }

        private void LoadCharacterStats()
        {
            //TODO: decide if we want to store all other loaded character stats OR only load the defined ID stats
            List<StatRecord> loadedStatData = LoadStats.LoadPlayerStats();
            Dictionary<Stat, float> baseStats;
            if (loadedStatData.Count - 1 >= _statID)
            {
                baseStats = loadedStatData[_statID].statDict;
            }
            else
            {
                baseStats = loadedStatData[0].statDict;
                Debug.LogWarning("For the set playerID no data is available. Loaded default player instead.");
            }
            base.Initialize(baseStats);
        }

        protected override void HandleDeath(GameObject self)
        {
            // Call OnDisable to ensure proper unsubscription of callbacks
            OnDisable();

            // Invoke OnDeath action
            OnDeath?.Invoke();
        }

        //this is just for reference to see how it could be done later
        public void SwitchPlayer(int playerId)
        {
            StatRecord playerData = LoadStats.LoadPlayerStats()[playerId];
            _stats.SetNewStatData(playerData.statDict);
        }
    }
}