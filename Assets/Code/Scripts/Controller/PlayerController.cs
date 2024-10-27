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
using UnityEditor.PackageManager;

namespace Controller
{
    public class PlayerController : BaseController
    {
        [SerializeField] private int playerID = 0;

        private PlayerMover mover;
        private EnemyFinderAll enemyFinder;

        private ItemInventory inventory;
        public ItemInventory Inventory => inventory;

        public Action onDeath;
        private Sanity sanity;

        protected override void Awake()
        {
            base.Awake();
            mover = GetComponent<PlayerMover>();
            enemyFinder = GetComponentInChildren<EnemyFinderAll>();
            sanity = GetComponent<Sanity>();
            inventory = GetComponentInChildren<ItemInventory>();
        }

        private void Start()
        {
            LoadCharacterStats();

            mover.Initialize(stats);
            enemyFinder.Initialize(stats);
            sanity.Initialize(stats); //TODO: connect SanityDecSpeed
            inventory.Initialize(gameObject);

            MountWeapon();
        }

        private void LoadCharacterStats()
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
        }

        private void MountWeapon()
        {
            WeaponHolder holder = gameObject.GetComponentInChildren<WeaponHolder>();
            if (holder != null && startWeapon != null)
            {
                Weapon weapon = Instantiate(startWeapon, holder.transform);
                weapon.Initialize(this, stats);
            }
            else Debug.LogError("Player character has no weapon older or weapon assigned.");
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