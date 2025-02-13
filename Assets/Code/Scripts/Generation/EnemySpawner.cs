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
        [SerializeField] private EnemySpawnConfig _spawnConfig;

        private List<StatRecord> _enemyInfo = new();
        private float _timer = 0;
        private Vector2 _spawnArea;
        private ObjectPool _enemyPool;

        private void Awake()
        {
            _enemyPool = GetComponent<ObjectPool>();
            _enemyInfo = LoadStats.LoadEnemyStats();
            _spawnArea = FindFirstObjectByType<RoomGenerator>().RoomSize;
        }

        private void Start()
        {
            StartCoroutine(SpawnEnemiesOverTime());
        }

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

        IEnumerator SpawnEnemies(SpawnWave wave)
        {
            //Todo: use pool for testers
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
                    for (int j = 0; j < wave.GroupSize; j++)
                    {
                        //find random point close to the spawn area
                        Vector3 spawnPoint;
                        if (wave.GroupSize == 1) spawnPoint = spawnArea;
                        else foundValidPoint = GetRandomSpawnPointInArea(spawnArea, out spawnPoint);
                        if (!foundValidPoint) continue;

                        //spawn tester to check if player is near
                        SpawnTester spawnTester = Instantiate(_spawnTesterPrefab, spawnPoint, Quaternion.identity, transform);
                        StartCoroutine(spawnTester.SpawningEnemy(spawnPoint, Spawn));
                    }
                }
                Debug.Log("Spawned Enemies");
                yield return new WaitForSeconds(wave.SpawnInterval);
            }
        }

        public void Spawn(Vector3 spawnPosition)
        {
            GameObject enemy = _enemyPool.GetObject();
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
            return _enemyInfo[Random.Range(0, _enemyInfo.Count)];
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