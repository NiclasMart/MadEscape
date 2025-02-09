// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 09/02/25
// Author: langner.paul@t-online.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ProgressTimer : MonoBehaviour
    {
        [SerializeField] public float LevelTime;
        private float _timeLeft;
        private void Start()
        {
            _timeLeft = LevelTime;
        }
        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            SetStatDisplay(_timeLeft);
            if ( _timeLeft < 0 )
            {
                LevelOver();
            }
        }

        private void LevelOver()
        {
            throw new NotImplementedException();
        }

        public void SetStatDisplay(float value)
        {
            int secondsLeft = (int) Math.Ceiling(value);
            GetComponent<TextMeshProUGUI>().text = secondsLeft.ToString("D");
        }
    }
}