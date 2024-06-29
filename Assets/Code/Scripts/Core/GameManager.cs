// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 10.08.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;
using System;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private PlayerController player;
        private SceneManagement sceneManagement;

        private void Awake()
        {
            //create Singelton
            if (Instance != null && Instance != this)
            {
                Destroy(Instance);
                return;
            }

            Instance = this;

            sceneManagement = GetComponent<SceneManagement>();
            player.onDeath += RestartGame;
        }

        private void RestartGame()
        {
            Debug.Log("Player is dead. Reloading scene");
            sceneManagement.ReloadScenes();
        }
    }
}