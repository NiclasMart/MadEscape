using Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes
{
    [MenuItem("Game Tools/Load Scene/Main")]
    public static void LoadMain()
    {
        CheckForChanges();
        EditorSceneManager.OpenScene("Assets/Content/Scenes/Main.unity");
        EditorSceneManager.OpenScene("Assets/Content/Scenes/TestEnemyAI.unity", OpenSceneMode.Additive);
    }

    [MenuItem("Game Tools/Load Scene/Weapon Test")]
    public static void LoadWeaponTest()
    {
        CheckForChanges();
        EditorSceneManager.OpenScene("Assets/Content/Scenes/Weapon_Test.unity");
    }

    private static void CheckForChanges()
    {
        bool unsafedChanges = false;
        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
            if (EditorSceneManager.GetSceneAt(i).isDirty)
            {
                unsafedChanges = true;
            }
        }

        if (!unsafedChanges) return;

        bool shouldSave = EditorUtility.DisplayDialog(
                    "Unsaved Changes",
                    "You have unsaved changes. Do you want to save them before switching scenes?",
                    "Save", // Button 1
                    "Don't Save" // Button 2
        );

        if (shouldSave)
        {
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

    }

}
