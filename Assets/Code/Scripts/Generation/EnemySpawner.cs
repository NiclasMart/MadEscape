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
using Generator;
using UnityEngine.AI;

namespace Generation
{
    [RequireComponent(typeof(ObjectPool))]
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<Item> lootTable = new();
        [SerializeField] private float spawnInterval = 3f;

        private List<StatRecord> enemyInfo = new();
        private float timer = 0;
        private Vector2 spawnArea;

        private ObjectPool enemyPool;

        private void Awake()
        {
            enemyPool = GetComponent<ObjectPool>();
            enemyInfo = LoadStats.LoadEnemyStats();
            spawnArea = FindFirstObjectByType<RoomGenerator>().roomSize;
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

        private Vector3 GetRandomSpawnPoint()
        {
            float y = (spawnArea.y - 1) / 2f * Random.Range(-1f, 1f);
            float x = (spawnArea.x - 1) / 2f * Random.Range(-1f, 1f);

            if (NavMesh.SamplePosition(new Vector3(x, 0, y), out NavMeshHit hit, 1, NavMesh.AllAreas))
            {
                return hit.position;
            }
            return transform.position;
        }
    }
}