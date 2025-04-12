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
        EditorSceneManager.OpenScene("Assets/Content/Scenes/Main.unity");
        EditorSceneManager.OpenScene("Assets/Content/Scenes/TestEnemyAI.unity", OpenSceneMode.Additive);
    }

    [MenuItem("Game Tools/Load Scene/Weapon Test")]
    public static void LoadWeaponTest()
    {
        EditorSceneManager.OpenScene("Assets/Content/Scenes/Weapon_Test.unity");
    }
}
