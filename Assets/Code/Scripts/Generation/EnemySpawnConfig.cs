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
    [HideInInspector] public string WaveName; // Optional: Name for debugging
    [HideInInspector] public bool IsSingleWave; // If true, this wave will only spawn once
    [HideInInspector] public int SpawnStartTime; // When this wave starts within the 90s
    [HideInInspector] public int SpawnEndTime; // When this wave ends
    [HideInInspector] public int SpawnInterval = 5; // Time between enemy spawns
    [HideInInspector] public int AmountOfGroupAreas = 4; // areas in room where groups of enemies can spawn
    [HideInInspector] public int GroupSize = 3; // amount of enemies in a group
    [HideInInspector] public int GroupSizeVariance = 1; // variance in group size
    [HideInInspector] public List<EnemySpawnInfo> EnemiesToSpawn;
}

[Serializable]
public class EnemySpawnInfo
{
    public GameObject EnemyPrefab;
    public int RelativAmount = 1;
}