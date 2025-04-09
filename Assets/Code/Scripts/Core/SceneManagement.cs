using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneManagement : MonoBehaviour
    {
        [SerializeField] private List<int> _loadedScenes = new();

        private void Awake()
        {
            ServiceProvider.Register(this, gameObject);
            //only load scene list if the game was started with the main scene
            if (SceneManager.GetActiveScene().buildIndex != 0) return;

            LoadScenes();
        }

        public void ReloadScenes()
        {
            ReloadAllActiveScenes();
        }

        private void ReloadAllActiveScenes()
        {
            // Get a list of all currently active scenes
            int sceneCount = SceneManager.sceneCount;
            var loadedScenes = new List<Scene>();

            for (int i = 0; i < sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i));
            }

            // Store the name of the active scene
            string activeSceneName = SceneManager.GetActiveScene().name;

            // Unload all active scenes
            var unloadOperations = new List<AsyncOperation>();
            foreach (var scene in loadedScenes)
            {
                Debug.Log($"Unloading scene: {scene.name}");
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(scene);
                if (unloadOperation != null)
                {
                    unloadOperations.Add(unloadOperation);
                }
            }

            // Wait for all unload operations to complete
            bool allUnloaded = false;
            while (!allUnloaded)
            {
                allUnloaded = true;
                foreach (var operation in unloadOperations)
                {
                    if (!operation.isDone)
                    {
                        allUnloaded = false;
                        break;
                    }
                }
            }

            // Reload all previously active scenes
            foreach (var scene in loadedScenes)
            {
                Debug.Log($"Reloading scene: {scene.name}");
                SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
            }

            // Restore the active scene
            Scene activeScene = SceneManager.GetSceneByName(activeSceneName);
            SceneManager.SetActiveScene(activeScene);
        }

        private void LoadScenes()
        {
            foreach (var scene in _loadedScenes)
            {
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            }
        }

        private void UnloadActiveScenes()
        {
            foreach (var scene in _loadedScenes)
            {
                SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            }
        }
    }
}