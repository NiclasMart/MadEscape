// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 24.07.24
// Author: niclas.mart@telekom.de
// Origin Project: 
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using Generation;
using Stats;
using UnityEngine;

namespace Combat
{
    public class EnemyFinderAll : MonoBehaviour
    {
        GameObject closestEnemy;
        private Transform enemiesParent;
        private float attackRange;

        private void Start()
        {
            enemiesParent = GameObject.Find("Pool").transform;
            if (enemiesParent == null)
            {
                Debug.LogError("EnemiesParent not found in the scene!");
            }
        }

        internal void Initialize(CharacterStats stats)
        {
            attackRange = stats.GetStat(Stat.AttackRange);
            stats.onStatsChanged += UpdateAttackRange;
        }

        public GameObject GetClosestEnemy()
        {
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

            return closestEnemy;
        }

        private void UpdateAttackRange(Stat stat, float newValue)
        {
            if (stat != Stat.AttackRange) return;
            attackRange = newValue;
        }
    }
}