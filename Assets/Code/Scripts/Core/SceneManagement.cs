using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneManagement : MonoBehaviour, IService
    {
        private List<int> _loadedScenes = new();

        public IEnumerator ReloadCurrentScenes()
        {
            // Create a list to store the names of all currently loaded scenes
            int sceneCount = SceneManager.sceneCount;
            var loadedSceneNames = new List<string>();

            // Store the names of all currently loaded scenes
            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.IsValid() && !string.IsNullOrEmpty(scene.name))
                {
                    loadedSceneNames.Add(scene.name);
                }
            }

            // Reload the active scene in Single mode
            string activeSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(activeSceneName, LoadSceneMode.Single);

            // Reload all other scenes in Additive mode
            foreach (string sceneName in loadedSceneNames)
            {
                if (sceneName != activeSceneName) // Skip the active scene as it is already reloaded
                {
                    SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                }
            }

            yield return null; // Wait for the scenes to load
        }
    }
}