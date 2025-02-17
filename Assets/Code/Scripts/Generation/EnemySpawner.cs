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
    public class EnemySpawner : MonoBehaviour
    {
        const float GROUP_AREA_SIZE = 3f;
        [SerializeField] private SpawnTester _spawnTesterPrefab;
        [SerializeField] private EnemySpawnConfig _spawnConfig;

        private LootTable _lootTable;
        private LootGenerator _lootGenerator;
        private List<StatRecord> _enemyInfo = new();
        private float _timer = 0;
        private Vector2 _spawnArea;
        private Dictionary<int, ObjectPool> _enemyPools = new();
        private ObjectPool _spawnTesterPool;

        private void Awake()
        {
            _enemyInfo = LoadStats.LoadEnemyStats();
            _spawnArea = FindFirstObjectByType<RoomGenerator>().RoomSize;
            _spawnTesterPool = CreateNewSpawnPool(_spawnTesterPrefab.gameObject, 10);
            _lootGenerator = new LootGenerator(_spawnConfig.LootTable);
        }

        private void Start()
        {
            StartCoroutine(SpawnEnemiesOverTime());
        }

        private ObjectPool CreateNewSpawnPool(GameObject poolType, int initialCapacity)
        {
            var poolHolder = new GameObject("Pool");
            poolHolder.transform.parent = transform;
            var pool = poolHolder.AddComponent<ObjectPool>();
            pool.Initialize(poolType, initialCapacity, poolHolder.transform);
            return pool;
        }

        //handles spawning of different waves
        IEnumerator SpawnEnemiesOverTime()
        {
            var delay = new WaitForSeconds(1f);
            SpawnWave currentWave = null;

            while (_timer < _spawnConfig.TotalDuration)
            {
                if (currentWave == null || currentWave.SpawnEndTime < _timer)
                {
                    currentWave = _spawnConfig.SpawnWaves.Find(wave => wave.SpawnStartTime <= _timer && wave.SpawnEndTime >= _timer);
                    if (currentWave != null)
                    {
                        Debug.Log($"Current Wave: {currentWave.WaveName}; Current Time: {_timer}; Current real time: {Time.time}");
                        StartCoroutine(SpawnEnemies(currentWave));
                    }

                }
                _timer += 1;
                yield return delay;
            }
        }

        //handles spawning of enemies in a wave
        IEnumerator SpawnEnemies(SpawnWave wave)
        {
            //loop defines how many groups of enemies are spawned
            while (_timer <= wave.SpawnEndTime)
            {
                for (int i = 0; i < wave.AmountOfGroupAreas; i++)
                {
                    //find random point in the room
                    bool foundValidPoint = false;
                    foundValidPoint = GetRandomSpawnPointInRoom(out Vector3 spawnArea);
                    if (!foundValidPoint) continue;

                    //loop defines how many enemies are spawned in a group
                    int groupSize = Random.Range(wave.GroupSize - wave.GroupSizeVariance, wave.GroupSize + wave.GroupSizeVariance);
                    for (int j = 0; j < groupSize; j++)
                    {
                        //find random point close to the spawn area
                        Vector3 spawnPoint;
                        if (wave.GroupSize == 1) spawnPoint = spawnArea;
                        else foundValidPoint = GetRandomSpawnPointInArea(spawnArea, out spawnPoint);
                        if (!foundValidPoint) continue;

                        //get enemy
                        EnemyController enemyType = wave.GetRandomEnemyTypeFromWave();
                        bool poolExists = _enemyPools.ContainsKey(enemyType.ID);
                        if (!poolExists)
                        {
                            _enemyPools.Add(enemyType.ID, CreateNewSpawnPool(enemyType.gameObject, 10));
                        }

                        //spawn tester to check if player is near
                        // SpawnTester spawnTester = Instantiate(_spawnTesterPrefab, spawnPoint, Quaternion.identity, transform);
                        SpawnTester spawnTester = _spawnTesterPool.GetObject().GetComponent<SpawnTester>();
                        spawnTester.transform.position = spawnPoint;
                        spawnTester.gameObject.SetActive(true);
                        StartCoroutine(spawnTester.SpawningEnemy(enemyType.ID, Spawn));
                    }
                }
                Debug.Log("Spawned Enemies");
                yield return new WaitForSeconds(wave.SpawnInterval);
            }
        }

        private void Spawn(Vector3 spawnPosition, int enemyTypeID)
        {
            GameObject enemy = _enemyPools[enemyTypeID].GetObject();
            SetUpEnemy(enemy, enemyTypeID);
            enemy.transform.position = spawnPosition;
            enemy.SetActive(true);
        }

        private void SetUpEnemy(GameObject enemy, int ID)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            StatRecord type = _enemyInfo.Find(info => info.id == ID);
            if (type == null)
            {
                Debug.LogError($"Enemy with ID {ID} not found in enemy info list.");
                return;
            }
            enemy.gameObject.name = type.name;

            Item drop = _lootGenerator.Generate();
            enemyController.Initialize(type.statDict, drop);
        }

        private bool GetRandomSpawnPointInRoom(out Vector3 point)
        {
            float x = (_spawnArea.x - 1) / 2f * Random.Range(-1f, 1f);
            float z = (_spawnArea.y - 1) / 2f * Random.Range(-1f, 1f);

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