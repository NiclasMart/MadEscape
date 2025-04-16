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
    public class GameManager : MonoBehaviour, IService
    {
        private PlayerController _player;
        private SceneManagement _sceneManagement;

        private void Awake()
        {
            _player = FindFirstObjectByType<PlayerController>();
            _sceneManagement = GetComponent<SceneManagement>();
            _player.OnDeath += RestartGame;
        }

        private void RestartGame()
        {
            Debug.Log("Player is dead. Reloading scene");
            _sceneManagement.ReloadScenes();
        }

        public PlayerController GetPlayer()
        {
            return _player;
        }
    }
}