// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 30.06.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using VitalForces;

namespace Combat
{
    public class EnemyFinder : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemies = new List<GameObject>();
        private Transform enemiesParent;

        float closestDistanceSqr;

        private SphereCollider detectionArea;

        private void Awake()
        {
            detectionArea = GetComponent<SphereCollider>();
        }

        internal void Initialize(CharacterStats stats)
        {
            detectionArea.radius = stats.GetStat(Stat.AttackRange);
            stats.onStatsChanged += UpdateAttackRange;
            if (enemiesParent == null)
            {   
                enemiesParent = GameObject.Find("Pool").transform;
                if (enemiesParent == null)
                {
                    Debug.LogError("EnemiesParent not found in the scene!");
                }
            }
        }

        public GameObject GetClosestEnemy()
        {
            GameObject closestEnemy = null;
            closestDistanceSqr = Mathf.Infinity;

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

            return closestEnemy;
        }

        public bool EnemyInRange()
        {
            return closestDistanceSqr < Math.Pow(detectionArea.radius,2);
        }

        private void UpdateAttackRange(Stat stat, float newValue)
        {
            if (stat != Stat.AttackRange) return;
            detectionArea.radius = newValue;
        }
    }
}