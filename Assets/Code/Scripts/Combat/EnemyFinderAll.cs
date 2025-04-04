// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 24.07.24
// Author: niclas.mart@telekom.de
// Origin Project: 
// ---------------------------------------------
// -------------------------------------------*/

using System;
using Controller;
using Core;
using Generation;
using UnityEngine;

namespace Combat
{
    public class EnemyFinderAll : MonoBehaviour
    {
        private GameObject _overrideTarget;
        private Transform _enemiesParent;


        public void Initialize(GameObject target)
        {
            _overrideTarget = target;
            if (_overrideTarget == null)
            {
                _enemiesParent = FindFirstObjectByType<EnemySpawner>().transform;
                if (_enemiesParent == null)
                {
                    Debug.LogError("EnemiesParent not found in the scene!");
                }
            }
                
        }

        public void GetClosestEnemy(out GameObject closestEnemy, out float distance)
        {
            //handling for finder on enemies
            if (_overrideTarget != null)
            {
                closestEnemy = _overrideTarget;
                distance = Vector3.Distance(transform.position, _overrideTarget.transform.position);
                return;
            }

            //handling for finder on player
            closestEnemy = null;
            float closestDistanceSqr = Mathf.Infinity;

            foreach (Transform pool in _enemiesParent)
            {
                if (pool.GetComponent<ObjectPool>() == null) continue;
                foreach (Transform enemy in pool)
                {
                    if (!enemy.gameObject.activeSelf || enemy.GetComponent<EnemyController>() == null) continue;

                    float distanceSqr = (enemy.position - transform.position).sqrMagnitude;
                    if (distanceSqr < closestDistanceSqr)
                    {
                        closestEnemy = enemy.gameObject;
                        closestDistanceSqr = distanceSqr;
                    }
                }
            }
            distance = (float)Math.Sqrt(closestDistanceSqr);
        }
    }
}