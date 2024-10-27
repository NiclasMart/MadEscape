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
        [SerializeField] protected Weapon startWeapon;
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

        abstract protected void HandleDeath(GameObject self);
    }
}