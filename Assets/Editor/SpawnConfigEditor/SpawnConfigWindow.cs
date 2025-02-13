using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class SpawnConfigWindow : EditorWindow
{

    //Todo: generate timeline for visual representation of spawn waves
    //Todo: add loot table
    //Todo: add estimation for wave stats like enemy amount and damage

    private EnemySpawnConfig spawnConfig;
    private Vector2 scrollPosition;
    private List<bool> waveFoldouts = new List<bool>();

    [MenuItem("Game Tools/Spawn Configurator")]
    public static void ShowWindow()
    {
        GetWindow<SpawnConfigWindow>("Spawn Configurator");
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

        spawnConfig.TotalDuration = EditorGUILayout.DelayedIntField("Total Duration", spawnConfig.TotalDuration);
        GUILayout.Space(20);

        // Sort List Button
        if (GUILayout.Button("Sort List"))
        {
            GUI.FocusControl(null);
            spawnConfig.SpawnWaves = spawnConfig.SpawnWaves.OrderBy(wave => wave.SpawnStartTime).ToList();
        }

        //handle foldouts
        if (waveFoldouts.Count != spawnConfig.SpawnWaves.Count)
        {
            waveFoldouts.Clear();
            for (int i = 0; i < spawnConfig.SpawnWaves.Count; i++)
            {
                waveFoldouts.Add(false);
            }
        }

        // Scroll View for long lists
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Loop through Waves
        for (int i = 0; i < spawnConfig.SpawnWaves.Count; i++)
        {
            var wave = spawnConfig.SpawnWaves[i];
            waveFoldouts[i] = EditorGUILayout.Foldout(waveFoldouts[i], $"Wave {i + 1}: starts at {wave.SpawnStartTime}s - {wave.WaveName}");

            if (waveFoldouts[i])
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                wave.WaveName = EditorGUILayout.TextField("Wave Name", wave.WaveName);
                wave.SpawnStartTime = Mathf.Clamp(EditorGUILayout.DelayedFloatField("Start Time", wave.SpawnStartTime), 0, spawnConfig.TotalDuration);
                wave.SpawnEndTime = Mathf.Clamp(EditorGUILayout.DelayedFloatField("End Time", wave.SpawnEndTime), wave.SpawnStartTime, spawnConfig.TotalDuration);
                wave.SpawnInterval = Mathf.Max(1, EditorGUILayout.DelayedFloatField("Spawn Interval", wave.SpawnInterval));
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

                if (GUILayout.Button("+ Add Enemy"))
                {
                    wave.EnemiesToSpawn.Add(new EnemySpawnInfo());
                }

                if (GUILayout.Button("- Remove Wave"))
                {
                    spawnConfig.SpawnWaves.RemoveAt(i);
                    waveFoldouts.RemoveAt(i);
                    break;
                }

                EditorGUILayout.EndVertical();
                GUILayout.Space(10);

            }
        }

        if (GUILayout.Button("+ Add Wave"))
        {
            spawnConfig.SpawnWaves.Add(new SpawnWave() { WaveName = "New Wave", EnemiesToSpawn = new List<EnemySpawnInfo>() });
            waveFoldouts.Add(false);
        }

        EditorGUILayout.EndScrollView();

        //show warning if waves 
        if (AreWaveTimesValid(out string message))
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

    private bool AreWaveTimesValid(out string message)
    {
        foreach (var wave in spawnConfig.SpawnWaves)
        {
            foreach (var otherWave in spawnConfig.SpawnWaves)
            {
                if (wave == otherWave) continue;
                if (wave.SpawnStartTime < otherWave.SpawnEndTime && wave.SpawnEndTime > otherWave.SpawnStartTime)
                {
                    message = $"Wave {wave.WaveName} overlaps with {otherWave.WaveName}";
                    return true;
                }

                if (wave.SpawnStartTime > spawnConfig.TotalDuration || wave.SpawnEndTime > spawnConfig.TotalDuration)
                {
                    message = $"Wave {wave.WaveName} exceeds Total Duration";
                    return true;
                }
            }
        }
        message = "";
        return false;
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
