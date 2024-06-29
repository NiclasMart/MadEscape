using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneManagement : MonoBehaviour
    {
        [SerializeField] private List<int> loadedScenes = new List<int>();

        private void Awake()
        {
            //only load scene list if the game was started with the main scene
            if (SceneManager.GetActiveScene().buildIndex != 0) return;

            LoadScenes();
        }

        public void ReloadScenes()
        {
            //Unload all additively loaded scenes
            UnloadActiveScenes();

            //reload main scene
            SceneManager.LoadSceneAsync(0);
        }

        private void LoadScenes()
        {
            foreach (var scene in loadedScenes)
            {
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            }
        }

        private void UnloadActiveScenes()
        {
            foreach (var scene in loadedScenes)
            {
                SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            }
        }
    }
}