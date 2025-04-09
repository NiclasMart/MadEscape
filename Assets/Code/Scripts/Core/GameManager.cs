// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 10.08.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine;
using Controller;
using UI;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private GameOverScreen _gameOverScreen;

        private void Awake()
        {
            ServiceProvider.Register(this, gameObject);

            _player.OnDeath += ShowGameOverScreen;
        }

        private void ShowGameOverScreen()
        {
            FreezeTime();
            _gameOverScreen.ShowGameOverScreen();
            _player.OnDeath -= ShowGameOverScreen; // Unsubscribe to prevent multiple calls
        }

        public void RestartGame()
        {
            SceneManagement _sceneManagement = ServiceProvider.Get<SceneManagement>();
            Debug.Log("Reloading scene");
            _sceneManagement.ReloadScenes();
            UnfreezeTime();
        }

        public void FreezeTime()
        {
            // Set the time scale to zero to freeze the game
            Time.timeScale = 0f;
            Debug.Log("Game time frozen.");
        }

        public void UnfreezeTime()
        {
            // Reset the time scale to 1 to resume the game
            Time.timeScale = 1f;
            Debug.Log("Game time resumed.");
        }
    }
}