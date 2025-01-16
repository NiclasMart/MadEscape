// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 16.01.25
// Author: melonanas1@gmail.com
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera mainCamera;
        void Start()
        {
            mainCamera = Camera.main;
        }

       void LateUpdate()
        {
        if (mainCamera != null)
        {
            // Die Health Bar so drehen, dass sie zur Kamera zeigt
            transform.forward = mainCamera.transform.forward;
            transform.Rotate(90f, 0f, 0f);
        }
        }
    }
}