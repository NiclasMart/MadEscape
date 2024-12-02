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
        [SerializeField] protected WeaponTemplate startWeapon;
        protected CharacterStats stats;
        protected Health health;


        protected virtual void Awake()
        {
            stats = GetComponent<CharacterStats>();
            health = GetComponent<Health>();
        }

        public void Initialize(Dictionary<Stat, float> baseStats)
        {
            stats.Initialize(baseStats);
            health.Initialize(stats, HandleDeath);
        }

        protected void MountWeapon(GameObject target)
        {
            WeaponController weaponController = gameObject.GetComponentInChildren<WeaponController>();
            if (weaponController != null && startWeapon != null)
            {
                weaponController.Initialize(target);
                weaponController.EquipNewWeapon(startWeapon);
            }
        }

        abstract protected void HandleDeath(GameObject self);
    }
}