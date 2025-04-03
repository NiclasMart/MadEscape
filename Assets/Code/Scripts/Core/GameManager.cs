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
        [SerializeField] private PlayerController _player;
        private SceneManagement _sceneManagement;

        private void Awake()
        {
            if (ServiceProvider.Get<GameManager>() != null)
            {
                Destroy(gameObject);
                return;
            }

            ServiceProvider.Register(this);
            DontDestroyOnLoad(gameObject);

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