// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 23.07.24
// Author: langner.paul@t-online.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections.Generic;
using Combat;
using Stats;
using UnityEngine;
using VitalForces;

namespace Controller
{
    public abstract class BaseController : MonoBehaviour
    {
        [SerializeField] protected int _statID = 0;
        public int ID => _statID;
        [SerializeField] protected WeaponTemplate _startWeapon;
        protected CharacterStats _stats;
        public Health Health { get; private set; }

        protected virtual void Awake()
        {
            _stats = GetComponent<CharacterStats>();
            Health = GetComponent<Health>();
        }

        protected virtual void OnEnable()
        {
            // Subscribe to callbacks when the object is enabled
            if (Health != null)
            {
                Health.OnDeath += HandleDeath;
            }
        }

        protected virtual void OnDisable()
        {
            // Unsubscribe from callbacks when the object is disabled
            if (Health != null)
            {
                Health.OnDeath -= HandleDeath;
            }
        }

        public void Initialize(Dictionary<Stat, float> baseStats)
        {
            _stats.Initialize(baseStats);
            Health.Initialize(_stats);
        }

        protected Weapon MountWeapon(GameObject target, string targetLayer)
        {
            WeaponController weaponController = gameObject.GetComponentInChildren<WeaponController>();
            if (weaponController != null && _startWeapon != null)
            {
                weaponController.SetTarget(target);
                return weaponController.InitWeapon(_startWeapon, targetLayer);
            }
            return null;
        }

        abstract protected void HandleDeath(GameObject self);
    }
}