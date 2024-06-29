using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Helper
{
    public class MainSceneLoadingHelper : MonoBehaviour
    {
        [SerializeField] private bool active = true;

        private void Awake()
        {
        }

        private IEnumerator LoadingAdditiveScenes()
        {
            yield return new WaitForEndOfFrame();

            bool isLoadedByMainScene = SceneManager.GetActiveScene().buildIndex == 0;
            if (active && !isLoadedByMainScene)
            {
                Debug.Log("Load Scene 2");
                SceneManager.LoadScene(0, LoadSceneMode.Additive);
            }
        }
    }
}