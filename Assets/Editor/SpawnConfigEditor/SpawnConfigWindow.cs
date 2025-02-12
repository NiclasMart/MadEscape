using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class SpawnConfigWindow : EditorWindow
{
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

        // Sort List Button
        if (GUILayout.Button("Sort List"))
        {
            if (spawnConfig == null) return;
            GUI.FocusControl(null);
            spawnConfig.spawnWaves = spawnConfig.spawnWaves.OrderBy(wave => wave.SpawnStartTime).ToList();
        }

        //check if spawnConfig is assigned
        if (spawnConfig == null)
        {
            EditorGUILayout.HelpBox("Please assign a Spawn Configuration", MessageType.Warning);
            return;
        }

        //handle foldouts
        if (waveFoldouts.Count != spawnConfig.spawnWaves.Count)
        {
            waveFoldouts.Clear();
            for (int i = 0; i < spawnConfig.spawnWaves.Count; i++)
            {
                waveFoldouts.Add(false);
            }
        }

        // Scroll View for long lists
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Loop through Waves
        for (int i = 0; i < spawnConfig.spawnWaves.Count; i++)
        {
            var wave = spawnConfig.spawnWaves[i];
            waveFoldouts[i] = EditorGUILayout.Foldout(waveFoldouts[i], $"Wave {i + 1}: starts at {wave.SpawnStartTime}s - {wave.WaveName}");

            if (waveFoldouts[i])
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                wave.WaveName = EditorGUILayout.TextField("Wave Name", wave.WaveName);
                wave.SpawnStartTime = Mathf.Max(0, EditorGUILayout.FloatField("Start Time", wave.SpawnStartTime));
                wave.SpawnEndTime = Mathf.Max(wave.SpawnStartTime, EditorGUILayout.FloatField("End Time", wave.SpawnEndTime));
                wave.SpawnInterval = EditorGUILayout.FloatField("Spawn Interval", wave.SpawnInterval);
                wave.AmountOfGroupAreas = EditorGUILayout.IntField("Group Areas", wave.AmountOfGroupAreas);
                wave.GroupSize = EditorGUILayout.IntField("Group Size", wave.GroupSize);
                wave.GroupSizeVariance = EditorGUILayout.IntField("Group Size Variance", wave.GroupSizeVariance);

                // Enemy Spawn List
                for (int j = 0; j < wave.EnemiesToSpawn.Count; j++)
                {
                    var enemyInfo = wave.EnemiesToSpawn[j];

                    EditorGUILayout.BeginHorizontal();

                    enemyInfo.EnemyPrefab = (GameObject)EditorGUILayout.ObjectField("Enemy", enemyInfo.EnemyPrefab, typeof(GameObject), false);
                    enemyInfo.RelativAmount = EditorGUILayout.IntField("Amount", enemyInfo.RelativAmount);

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
                    spawnConfig.spawnWaves.RemoveAt(i);
                    waveFoldouts.RemoveAt(i);
                    break;
                }

                EditorGUILayout.EndVertical();
                GUILayout.Space(10);

            }
        }

        if (GUILayout.Button("+ Add Wave"))
        {
            spawnConfig.spawnWaves.Add(new SpawnWave() { WaveName = "New Wave", EnemiesToSpawn = new List<EnemySpawnInfo>() });
            waveFoldouts.Add(false);
        }

        EditorGUILayout.EndScrollView();

        if (WavesOverlap())
        {
            EditorGUILayout.HelpBox("In the list are waves that overlap!", MessageType.Warning);
            return;
        }

        // Save changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(spawnConfig);
        }
    }

    private bool WavesOverlap()
    {
        foreach (var wave in spawnConfig.spawnWaves)
        {
            foreach (var otherWave in spawnConfig.spawnWaves)
            {
                if (wave == otherWave) continue;
                if (wave.SpawnStartTime < otherWave.SpawnEndTime && wave.SpawnEndTime > otherWave.SpawnStartTime)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
