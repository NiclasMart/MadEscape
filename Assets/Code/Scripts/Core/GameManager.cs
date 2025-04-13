// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 10.08.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections; // Add this to resolve the IEnumerator error
using UnityEngine;
using Controller;
using UI;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private GameOverScreen _gameOverScreen;
        [SerializeField] private ProgressTimer _progressTimer;

        private void Awake()
        {
            ServiceProvider.Register(this, gameObject);

            _player.OnDeath += ShowGameOverScreen;
            _progressTimer.onTimerEnded += ShowGameOverScreen;
        }

        private void ShowGameOverScreen()
        {
            FreezeTime();
            _gameOverScreen.ShowGameOverScreen();
            _player.OnDeath -= ShowGameOverScreen; // Unsubscribe to prevent multiple calls
        }

        public void RestartGame()
        {
            StartCoroutine(RestartGameCoroutine());
        }

        private IEnumerator RestartGameCoroutine()
        {
            SceneManagement _sceneManagement = ServiceProvider.Get<SceneManagement>();
            yield return _sceneManagement.ReloadCurrentScenes(); // Wait for scenes to reload
            UnfreezeTime(); // Unfreeze time after reloading is complete
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