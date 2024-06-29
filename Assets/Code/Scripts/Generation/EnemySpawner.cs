// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 25.06.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// Info: Place spawn locations (empty transform game objects) as child objects. Out of those, the spawner finds a random one and spawns an enemy.
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using Core;
using Controller;
using Items;
using Stats;

namespace Generation
{
    [RequireComponent(typeof(ObjectPool))]
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<Item> lootTable = new();
        [SerializeField] private float spawnInterval = 3f;

        private List<StatRecord> enemyInfo = new();
        private float timer = 0;

        private ObjectPool enemyPool;

        private void Awake()
        {
            enemyPool = GetComponent<ObjectPool>();
            enemyInfo = LoadStats.LoadEnemyStats();
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
            StatRecord type = GetRandomEnemyType();
            enemy.gameObject.name = type.name;
            enemyController.Initialize(type.statDict, lootTable);
        }

        private StatRecord GetRandomEnemyType()
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