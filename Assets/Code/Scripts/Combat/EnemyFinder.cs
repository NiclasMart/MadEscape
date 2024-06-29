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

namespace Combat
{
    public class EnemyFinder : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemies = new List<GameObject>();

        private SphereCollider detectionArea;

        private void Awake()
        {
            detectionArea = GetComponent<SphereCollider>();
        }

        internal void Initialize(CharacterStats stats)
        {
            detectionArea.radius = stats.GetStat(Stat.AttackRange);
            stats.onStatsChanged += UpdateAttackRange;
        }

        public GameObject GetClosestEnemy()
        {
            GameObject closestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                if (closestEnemy == null) closestEnemy = enemy;
                else
                {
                    float currentClosestDistance = Vector3.SqrMagnitude(closestEnemy.transform.position - transform.position);
                    float newDistance = Vector3.SqrMagnitude(enemy.transform.position - transform.position);

                    if (newDistance < currentClosestDistance) closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                enemies.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                enemies.Remove(other.gameObject);
            }
        }
        private void UpdateAttackRange(Stat stat, float newValue)
        {
            if (stat != Stat.AttackRange) return;
            Debug.Log($"Updated Attack range from {detectionArea.radius} to {newValue}");
            detectionArea.radius = newValue;
        }
    }
}