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
using Generation;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ProgressTimer : MonoBehaviour
    {
        [SerializeField] private EnemySpawnConfig _spawnConfig;
        public event Action OnTimerEnded;
        private EnemySpawner _enemySpawner;
        private TextMeshProUGUI _textGui;

        private void Start()
        {
            _textGui = GetComponent<TextMeshProUGUI>();
            _enemySpawner = FindFirstObjectByType<EnemySpawner>();
            _enemySpawner.OnTimerUpdated += HandleTimerUpdated;
        }

        private void HandleTimerUpdated(float timer)
        {
            float timeLeft = _spawnConfig.TotalDuration - timer;
            SetTimeDisplay(timeLeft);
            if (timeLeft <= 0)
            {
                OnTimerEnded?.Invoke();
            }
        }

        public void SetTimeDisplay(float value)
        {
            int secondsLeft = (int)Math.Ceiling(value);
            _textGui.text = secondsLeft.ToString("D");
            if (secondsLeft <= 10)
            {
                _textGui.color = Color.red;
            }
        }
    }
}