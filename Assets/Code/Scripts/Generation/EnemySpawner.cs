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
using System.Collections;

namespace Generation
{
    [RequireComponent(typeof(ObjectPool))]
    public class EnemySpawner : MonoBehaviour
    {
        const float GROUP_AREA_SIZE = 3f;
        [SerializeField] private SpawnTester _spawnTesterPrefab;
        [SerializeField] private List<Item> _lootTable = new();

        [Header("Spawn Settings")]
        [SerializeField] private float _spawnInterval = 3f;
        [SerializeField][Min(1)] private int _spawnAmount = 1;
        [SerializeField][Min(1)] private int _groupSize = 1;

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
            if (timer > _spawnInterval)
            {
                SpawnEnemys(_spawnAmount, _groupSize);
                timer = 0;
            }
            timer += Time.deltaTime;
        }

        private void SpawnEnemys(int spawnAmount = 1, int goupSize = 1)
        {
            //Todo: use pool for testers
            //loop defines how many groups of enemies are spawned
            for (int i = 0; i < spawnAmount; i++)
            {
                //find random point in the room
                bool foundValidPoint = false;
                foundValidPoint = GetRandomSpawnPointInRoom(out Vector3 spawnArea);
                if (!foundValidPoint) continue;

                //loop defines how many enemies are spawned in a group
                for (int j = 0; j < goupSize; j++)
                {
                    //find random point close to the spawn area
                    Vector3 spawnPoint;
                    if (goupSize == 1) spawnPoint = spawnArea;
                    else foundValidPoint = GetRandomSpawnPointInArea(spawnArea, out spawnPoint);
                    if (!foundValidPoint) continue;

                    //spawn tester to check if player is near
                    SpawnTester spawnTester = Instantiate(_spawnTesterPrefab, spawnPoint, Quaternion.identity, transform);
                    StartCoroutine(spawnTester.SpawningEnemy(spawnPoint, Spawn));
                }
            }
        }

        public void Spawn(Vector3 spawnPosition)
        {
            GameObject enemy = enemyPool.GetObject();
            SetUpEnemy(enemy);
            enemy.transform.position = spawnPosition;
            enemy.SetActive(true);
        }

        private void SetUpEnemy(GameObject enemy)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            StatRecord type = GetRandomEnemyType();
            enemy.gameObject.name = type.name;
            enemyController.Initialize(type.statDict, _lootTable);
        }

        private StatRecord GetRandomEnemyType()
        {
            return enemyInfo[Random.Range(0, enemyInfo.Count)];
        }

        private bool GetRandomSpawnPointInRoom(out Vector3 point)
        {
            float x = (spawnArea.x - 1) / 2f * Random.Range(-1f, 1f);
            float z = (spawnArea.y - 1) / 2f * Random.Range(-1f, 1f);

            if (NavMesh.SamplePosition(new Vector3(x, 0, z), out NavMeshHit hit, 1, NavMesh.AllAreas))
            {
                point = hit.position;
                return true;
            }
            point = Vector3.zero;
            return false;
        }

        private bool GetRandomSpawnPointInArea(Vector3 area, out Vector3 point)
        {
            float x = area.x + Random.Range(-1f, 1f) * GROUP_AREA_SIZE;
            float z = area.z + Random.Range(-1f, 1f) * GROUP_AREA_SIZE;

            if (NavMesh.SamplePosition(new Vector3(x, 0, z), out NavMeshHit hit, 1, NavMesh.AllAreas))
            {
                point = hit.position;
                return true;
            }
            point = Vector3.zero;
            return false;
        }
    }
}