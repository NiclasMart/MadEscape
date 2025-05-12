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
    public class GameManager : MonoBehaviour, IService
    {
        private PlayerController _player;
        private GameOverScreen _gameOverScreen;
        private ProgressTimer _progressTimer;

        public void RestartGame()
        {
            StartCoroutine(RestartGameCoroutine());
        }

        public PlayerController GetPlayer()
        {
            return _player;
        }

        public void Load()
        {
            _player = FindFirstObjectByType<PlayerController>();
            _gameOverScreen = FindFirstObjectByType<GameOverScreen>(FindObjectsInactive.Include);
            _progressTimer = FindFirstObjectByType<ProgressTimer>();

            _player.OnDeath += ShowGameOverScreen;
            _progressTimer.OnTimerEnded += ShowGameOverScreen;
        }

        private void ShowGameOverScreen()
        {
            FreezeTime();
            _gameOverScreen.SetActive();

            _player.OnDeath -= ShowGameOverScreen;
            _progressTimer.OnTimerEnded -= ShowGameOverScreen;
        }

        private IEnumerator RestartGameCoroutine()
        {
            SceneManagement _sceneManagement = ServiceProvider.Get<SceneManagement>();
            yield return _sceneManagement.ReloadCurrentScenes();

            // reload all services after scene reload
            foreach (var service in ServiceProvider.GetAll())
            {
                service.Load();
            }
            ServiceProvider.Get<StatisticTracker>().ResetStatistics();

            UnfreezeTime();
        }

        private void FreezeTime()
        {
            Time.timeScale = 0f;
            Debug.Log("Game time frozen.");
        }

        private void UnfreezeTime()
        {
            Time.timeScale = 1f;
            Debug.Log("Game time resumed.");
        }
    }
}