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
        [SerializeField] protected WeaponTemplate _startWeapon;
        protected CharacterStats _stats;
        protected Health _health;


        protected virtual void Awake()
        {
            _stats = GetComponent<CharacterStats>();
            _health = GetComponent<Health>();
        }

        public void Initialize(Dictionary<Stat, float> baseStats)
        {
            _stats.Initialize(baseStats);
            _health.Initialize(_stats, HandleDeath);
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