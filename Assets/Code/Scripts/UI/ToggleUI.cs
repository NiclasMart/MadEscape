// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 10.01.24
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ToggleUI : MonoBehaviour
    {
        [SerializeField] bool setInvisibleOnStart = true;

        private void Start()
        {
            gameObject.SetActive(!setInvisibleOnStart);
        }

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}