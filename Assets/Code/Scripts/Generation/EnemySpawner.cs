// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 25.06.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// Info: Place spawn locations (empty transform game objects) as child objects. Out of those, the spawner finds a random one and spawns an enemy.
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Data;
using Controller;
using UnityEngine.AI;
using Items;

namespace Generation
{
    [RequireComponent(typeof(ObjectPool))]
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<EnemyData> enemyInfo = new List<EnemyData>();
        [SerializeField] private List<Item> lootTable = new List<Item>();
        [SerializeField] private float spawnInterval = 3f;

        private float timer = 0;

        private ObjectPool enemyPool;

        private void Awake()
        {
            enemyPool = GetComponent<ObjectPool>();
        }

        private void FixedUpdate()
        {
            //time spawn interval
            if (timer > spawnInterval)
            {
                SpawnEnemy();
                timer = 0;
            }
            timer += Time.deltaTime;
        }

        public void SpawnEnemy()
        {
            GameObject enemy = enemyPool.GetObject();

            SetUpEnemy(enemy);

            Vector3 spawnPosition = GetRandomSpawnPoint();
            enemy.transform.position = spawnPosition;
            enemy.SetActive(true);
        }

        private void SetUpEnemy(GameObject enemy)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            EnemyData type = GetRandomEnemyType();
            enemyController.Initialize(type.stats, lootTable);
            enemy.gameObject.name = type.typeName;
        }

        private EnemyData GetRandomEnemyType()
        {
            return enemyInfo[Random.Range(0, enemyInfo.Count)];
        }

        //Todo: generate spawn point dynamically
        private Vector3 GetRandomSpawnPoint()
        {
            int index = Random.Range(0, transform.childCount);
            return transform.GetChild(index).position;
        }
    }
}