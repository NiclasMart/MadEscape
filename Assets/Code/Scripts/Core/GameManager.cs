// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 10.08.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine;
using Controller;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private PlayerController _player;
        private SceneManagement _sceneManagement;

        private void Awake()
        {
            //create Singelton
            if (Instance != null && Instance != this)
            {
                Destroy(Instance);
                return;
            }

            Instance = this;

            _sceneManagement = GetComponent<SceneManagement>();
            _player.OnDeath += RestartGame;
        }

        private void RestartGame()
        {
            Debug.Log("Player is dead. Reloading scene");
            _sceneManagement.ReloadScenes();
        }
    }
}