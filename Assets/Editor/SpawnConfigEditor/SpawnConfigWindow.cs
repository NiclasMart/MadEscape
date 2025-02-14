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
    private List<bool> _waveFoldouts = new();

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

                if (GUILayout.Button("+ Add Enemy"))
                {
                    wave.EnemiesToSpawn.Add(new EnemySpawnInfo());
                }

                if (GUILayout.Button("- Remove Wave"))
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
