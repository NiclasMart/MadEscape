// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 24.07.24
// Author: niclas.mart@telekom.de
// Origin Project: 
// ---------------------------------------------
// -------------------------------------------*/

using System;
using Stats;
using UnityEngine;

namespace Combat
{
    public class EnemyFinderAll : MonoBehaviour
    {
        private GameObject overrideTarget;
        private Transform enemiesParent;


        public void Initialize(GameObject target)
        {
            overrideTarget = target;
            if (overrideTarget == null)
            {
                enemiesParent = GameObject.Find("Pool").transform;
                if (enemiesParent == null)
                {
                    Debug.LogError("EnemiesParent not found in the scene!");
                }
            }
                
        }

        public void GetClosestEnemy(out GameObject closestEnemy, out float distance)
        {
            //handling for finder on enemies
            if (overrideTarget != null)
            {
                closestEnemy = overrideTarget;
                distance = Vector3.Distance(transform.position, overrideTarget.transform.position);
                return;
            }

            //handling for finder on player
            closestEnemy = null;
            float closestDistanceSqr = Mathf.Infinity;

            foreach (Transform enemyTransform in enemiesParent)
            {
                if (!enemyTransform.gameObject.activeSelf) continue;

                float distanceSqr = (enemyTransform.position - transform.position).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestEnemy = enemyTransform.gameObject;
                    closestDistanceSqr = distanceSqr;
                }
            }
            distance = (float)Math.Sqrt(closestDistanceSqr);
        }
    }
}