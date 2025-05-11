// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 08.10.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using VitalForces;
using UnityEngine;
using System.Collections;

namespace Items
{
    public class LifeContainer : Item
    {
        [SerializeField] float _restoreAmount;

        public override void Use(GameObject user)
        {
            Health health = user.GetComponent<Health>();
            if (health)
            {
                health.StartCoroutine(health.RestoreHealthOverTime(_restoreAmount));
            }
            Destroy(this);
        }

        public override bool CollectConditionIsFullfilled(GameObject user)
        {
            Health health = user.GetComponent<Health>();
            return health && (health.ProportionalValue < (1 - Mathf.Epsilon));
        }
    }
}