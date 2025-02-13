using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnConfig", menuName = "ScriptableObjects/Spawn Configuration")]
public class EnemySpawnConfig : ScriptableObject
{
    public int TotalDuration = 90; // Total spawn cycle in seconds
    //Todo: add loot table
    public List<SpawnWave> SpawnWaves;
}

[Serializable]
public class SpawnWave
{
    public string WaveName; // Optional: Name for debugging
    public float SpawnStartTime; // When this wave starts within the 90s
    public float SpawnEndTime; // When this wave ends
    public float SpawnInterval = 5f; // Time between enemy spawns
    public int AmountOfGroupAreas = 4; // areas in room where groups of enemies can spawn
    public int GroupSize = 3; // amount of enemies in a group
    public int GroupSizeVariance = 1; // variance in group size
    public List<EnemySpawnInfo> EnemiesToSpawn;
}

[Serializable]
public class EnemySpawnInfo
{
    public GameObject EnemyPrefab;
    public int RelativAmount = 1;
}