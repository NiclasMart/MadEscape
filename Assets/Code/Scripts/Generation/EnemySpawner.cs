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
using Random = UnityEngine.Random;
using Core;
using Controller;
using Items;
using Stats;
using Generator;
using UnityEngine.AI;
using System.Collections;
using System;

namespace Generation
{
    public class EnemySpawner : MonoBehaviour
    {
        const float GROUP_AREA_SIZE = 3f;
        const int INITIAL_POOL_SIZE = 10;
        const int MAX_POOL_SIZE = 100;
        [SerializeField] private SpawnTester _spawnTesterPrefab;
        [SerializeField] private EnemySpawnConfig _spawnConfig;

        private LootGenerator _lootGenerator;
        private List<StatRecord> _enemyInfo = new();
        private float _timer = 0;
        private Vector2 _spawnArea;
        private Dictionary<int, ObjectPool> _enemyPools = new();
        private ObjectPool _spawnTesterPool;

        public event Action<float> OnTimerUpdated;

        private void Awake()
        {
            _enemyInfo = LoadStats.LoadEnemyStats();
            _spawnArea = FindFirstObjectByType<RoomGenerator>().RoomSize;
            _spawnTesterPool = CreateNewSpawnPool(_spawnTesterPrefab.gameObject, INITIAL_POOL_SIZE, MAX_POOL_SIZE);
            _lootGenerator = new LootGenerator(_spawnConfig.LootTable);
        }

        private void Start()
        {
            StartCoroutine(SpawnEnemiesOverTime());
        }

        private ObjectPool CreateNewSpawnPool(GameObject poolType, int initialCapacity, int maxCapacity)
        {
            var poolHolder = new GameObject("Pool");
            poolHolder.transform.parent = transform;
            var pool = poolHolder.AddComponent<ObjectPool>();
            pool.Initialize(poolType, poolHolder.transform, initialCapacity, maxCapacity);
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
                OnTimerUpdated?.Invoke(_timer);
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

                        //spawn tester to check if player is near
                        if (!_spawnTesterPool.TryGetObject(out GameObject pooledObject)) continue;

                        SpawnTester spawnTester = pooledObject.GetComponent<SpawnTester>();
                        spawnTester.transform.position = spawnPoint;
                        spawnTester.gameObject.SetActive(true);

                        //get enemy
                        EnemyController enemyType = wave.GetRandomEnemyTypeFromWave();
                        bool poolExists = _enemyPools.ContainsKey(enemyType.ID);
                        if (!poolExists)
                        {
                            _enemyPools.Add(enemyType.ID, CreateNewSpawnPool(enemyType.gameObject, INITIAL_POOL_SIZE, MAX_POOL_SIZE));
                        }

                        StartCoroutine(spawnTester.SpawningEnemy(enemyType.ID, Spawn));
                    }
                }
                Debug.Log("Spawned Enemies");
                yield return new WaitForSeconds(wave.SpawnInterval);
            }
        }

        private void Spawn(Vector3 spawnPosition, int enemyTypeID)
        {
            if (!_enemyPools[enemyTypeID].TryGetObject(out GameObject enemy)) return;

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