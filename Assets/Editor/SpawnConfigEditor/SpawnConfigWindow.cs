using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Items;
using Stats;
using Controller;

public class WaveData
{
    public bool isFolded = false;
    public int minEnemyAmount = 0;
    public int maxEnemyAmount = 0;
    public float waveLife = 0;
}

public class SpawnConfigWindow : EditorWindow
{

    //Todo: generate timeline for visual representation of spawn waves
    //Todo: add estimation for wave stats like enemy amount and damage

    private EnemySpawnConfig spawnConfig;
    private Vector2 scrollPosition;
    private List<bool> _waveFoldouts = new();
    private List<WaveData> _waveData = new();
    private List<StatRecord> _enemyInfo = new();



    [MenuItem("Game Tools/Spawn Configurator")]
    public static void ShowWindow()
    {
        GetWindow<SpawnConfigWindow>("Spawn Configurator");
    }

    private void OnEnable()
    {
        Debug.Log("load stats");
        _enemyInfo = LoadStats.LoadEnemyStats();
    }

    private void OnGUI()
    {
        GUILayout.Label("Enemy Spawn Configuration", EditorStyles.boldLabel);
        spawnConfig = (EnemySpawnConfig)EditorGUILayout.ObjectField("Spawn Config", spawnConfig, typeof(EnemySpawnConfig), false);

        //check if spawnConfig is assigned
        if (spawnConfig == null)
        {
            EditorGUILayout.HelpBox("Please assign a Spawn Configuration", MessageType.Warning);
            return;
        }

        // overall wave stats
        spawnConfig.TotalDuration = EditorGUILayout.DelayedIntField("Total Duration", spawnConfig.TotalDuration);
        spawnConfig.LootTable = (LootTable)EditorGUILayout.ObjectField("Loot Table", spawnConfig.LootTable, typeof(LootTable), false);

        GUILayout.Space(20);

        // Sort List Button
        if (GUILayout.Button("Sort List"))
        {
            GUI.FocusControl(null);
            spawnConfig.SpawnWaves = spawnConfig.SpawnWaves.OrderBy(wave => wave.SpawnStartTime).ToList();
        }

        // handle foldouts
        if (_waveFoldouts.Count != spawnConfig.SpawnWaves.Count)
        {
            _waveFoldouts.Clear();
            for (int i = 0; i < spawnConfig.SpawnWaves.Count; i++)
            {
                _waveFoldouts.Add(false);
            }
        }

        // Scroll View for long lists
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Loop through Waves
        for (int i = 0; i < spawnConfig.SpawnWaves.Count; i++)
        {
            var wave = spawnConfig.SpawnWaves[i];
            // if (_waveData.Count < i + 1) _waveData.Add(new WaveData() { isFolded = false});

            //create foldout with custom style
            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
            if (wave.IsSingleWave)
            {
                foldoutStyle.normal.textColor = new Color(1.0f, 0.5f, 0.5f);
                foldoutStyle.onNormal.textColor = new Color(1.0f, 0.5f, 0.5f);
                foldoutStyle.focused.textColor = new Color(1.0f, 0.5f, 0.5f);
                foldoutStyle.onFocused.textColor = new Color(1.0f, 0.5f, 0.5f);
            }
            else
            {
                foldoutStyle.onNormal.textColor = Color.white;
                foldoutStyle.focused.textColor = Color.white;
                foldoutStyle.onFocused.textColor = Color.white;

            }
            _waveFoldouts[i] = EditorGUILayout.Foldout(_waveFoldouts[i], $"Wave {i + 1}: starts at {wave.SpawnStartTime}s - {wave.WaveName}", foldoutStyle);

            // wave settings
            if (_waveFoldouts[i])
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                wave.WaveName = EditorGUILayout.TextField("Wave Name", wave.WaveName);
                wave.IsSingleWave = EditorGUILayout.Toggle("Single Wave", wave.IsSingleWave);
                wave.SpawnStartTime = Mathf.Clamp(EditorGUILayout.DelayedIntField("Start Time", wave.SpawnStartTime), 0, spawnConfig.TotalDuration);

                if (!wave.IsSingleWave)
                {
                    wave.SpawnEndTime = Mathf.Clamp(EditorGUILayout.DelayedIntField("End Time", wave.SpawnEndTime), wave.SpawnStartTime, spawnConfig.TotalDuration);
                    wave.SpawnInterval = Mathf.Max(1, EditorGUILayout.DelayedIntField("Spawn Interval", wave.SpawnInterval));
                }
                else
                {
                    wave.SpawnEndTime = wave.SpawnStartTime;
                    wave.SpawnInterval = 0;
                }

                wave.AmountOfGroupAreas = Mathf.Max(1, EditorGUILayout.DelayedIntField("Group Areas", wave.AmountOfGroupAreas));
                wave.GroupSize = Mathf.Max(1, EditorGUILayout.DelayedIntField("Group Size", wave.GroupSize));
                wave.GroupSizeVariance = Mathf.Max(0, EditorGUILayout.DelayedIntField("Group Size Variance", wave.GroupSizeVariance));
                GUILayout.Space(20);

                // Enemy Spawn List
                for (int j = 0; j < wave.EnemiesToSpawn.Count; j++)
                {
                    var enemyInfo = wave.EnemiesToSpawn[j];

                    EditorGUILayout.BeginHorizontal();

                    enemyInfo.EnemyPrefab = (GameObject)EditorGUILayout.ObjectField("Enemy", enemyInfo.EnemyPrefab, typeof(GameObject), false);
                    enemyInfo.RelativAmount = Mathf.Max(1, EditorGUILayout.DelayedIntField("Relative Amount", enemyInfo.RelativAmount));

                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        wave.EnemiesToSpawn.RemoveAt(j);
                        break;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                // wave stats
                GUILayout.BeginVertical(GUI.skin.box);
                GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.fontSize = 10;
                labelStyle.fontStyle = FontStyle.Italic;

                int minEnemyAmountPerSpawn = wave.AmountOfGroupAreas * (wave.GroupSize - wave.GroupSizeVariance);
                int maxEnemyAmountPerSpawn = wave.AmountOfGroupAreas * (wave.GroupSize + wave.GroupSizeVariance);
                int minEnemyAmountTotal = wave.IsSingleWave ? minEnemyAmountPerSpawn : (1 + (wave.SpawnEndTime - wave.SpawnStartTime) / wave.SpawnInterval) * minEnemyAmountPerSpawn;
                int maxEnemyAmountTotal = wave.IsSingleWave ? maxEnemyAmountPerSpawn : (1 + (wave.SpawnEndTime - wave.SpawnStartTime) / wave.SpawnInterval) * maxEnemyAmountPerSpawn;
                GUILayout.Label($"Total enemy amount: {minEnemyAmountTotal} - {maxEnemyAmountTotal}", labelStyle);
                float averageEnemies = (minEnemyAmountTotal + maxEnemyAmountTotal) / 2;
                int totalEnemyWeight = wave.EnemiesToSpawn.Sum(e => e != null ? e.RelativAmount : 0);
                double averageHealthPerEnemy = wave.EnemiesToSpawn.Sum(e =>
                {
                    if (e == null || e.EnemyPrefab == null) return 0;
                    var enemyController = e.EnemyPrefab.GetComponent<EnemyController>();
                    if (enemyController == null) return 0;
                    var enemyData = _enemyInfo.Find(enemyData => enemyData.id == enemyController.ID);
                    if (enemyData == null) return 0;
                    return e.RelativAmount / (double)totalEnemyWeight * enemyData.statDict[Stat.Life];
                });
                GUILayout.Label($"Average total wave life: {averageHealthPerEnemy * averageEnemies}", labelStyle);
                GUILayout.EndVertical();

                if (GUILayout.Button("+ Add Enemy"))
                {
                    wave.EnemiesToSpawn.Add(new EnemySpawnInfo());
                }

                GUILayout.Space(10);
                if (GUILayout.Button("Remove Wave"))
                {
                    spawnConfig.SpawnWaves.RemoveAt(i);
                    _waveFoldouts.RemoveAt(i);
                    break;
                }

                EditorGUILayout.EndVertical();
                GUILayout.Space(10);

            }
        }

        if (GUILayout.Button("+ Add Wave"))
        {
            spawnConfig.SpawnWaves.Add(new SpawnWave() { WaveName = "New Wave", EnemiesToSpawn = new List<EnemySpawnInfo>() });
            _waveFoldouts.Add(false);
        }

        EditorGUILayout.EndScrollView();

        //show warning if waves 
        if (!ValidateWaveStats(out string message))
        {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
            return;
        }

        //save button
        if (GUILayout.Button("Save Configuration"))
        {
            DeleteUnsetEnemies();
            EditorUtility.SetDirty(spawnConfig);
        }
    }

    private bool ValidateWaveStats(out string message)
    {
        foreach (var wave in spawnConfig.SpawnWaves)
        {
            foreach (var otherWave in spawnConfig.SpawnWaves)
            {
                if (wave == otherWave) continue;
                if (wave.SpawnStartTime < otherWave.SpawnEndTime && wave.SpawnEndTime > otherWave.SpawnStartTime)
                {
                    message = $"Wave {wave.WaveName} overlaps with {otherWave.WaveName}";
                    return false;
                }
            }
            if (wave.SpawnStartTime > spawnConfig.TotalDuration || wave.SpawnEndTime > spawnConfig.TotalDuration)
            {
                message = $"Wave {wave.WaveName} exceeds Total Duration";
                return false;
            }

            if (!wave.IsSingleWave && wave.SpawnStartTime == wave.SpawnEndTime)
            {
                message = $"Wave {wave.WaveName} is not a single wave but has the same start and end time";
                return false;
            }

            if (!wave.IsSingleWave && wave.SpawnEndTime - wave.SpawnStartTime < wave.SpawnInterval)
            {
                message = $"Wave {wave.WaveName} has a spawn interval that exceeds the wave duration";
                return false;
            }

            if (wave.EnemiesToSpawn.Count == 0)
            {
                message = $"Wave {wave.WaveName} has no enemies to spawn";
                return false;
            }

            foreach (var enemy in wave.EnemiesToSpawn)
            {
                if (enemy != null) continue;
                message = $"Wave {wave.WaveName} has an empty enemy within its spawn list";
                return false;
            }
        }
        message = "";
        return true;
    }

    private void DeleteUnsetEnemies()
    {
        for (int i = 0; i < spawnConfig.SpawnWaves.Count; i++)
        {
            var wave = spawnConfig.SpawnWaves[i];
            for (int j = 0; j < wave.EnemiesToSpawn.Count; j++)
            {
                if (wave.EnemiesToSpawn[j].EnemyPrefab == null)
                {
                    wave.EnemiesToSpawn.RemoveAt(j);
                    j--;
                }
            }
        }
    }
}
