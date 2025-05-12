// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 05.04.25
// Author: langner.paul@t-online.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        public void SetActive()
        {
            gameObject.SetActive(true);
        }

        public void RestartButton()
        {
            // Use the existing GameManager's RestartGame function
            GameManager gameManager = ServiceProvider.Get<GameManager>();
            gameManager.RestartGame();
        }

        public void ExitButton()
        {
            // Quit the application or return to the main menu
            Application.Quit();
        }
    }
}