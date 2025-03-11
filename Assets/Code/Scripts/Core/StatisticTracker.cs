// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 11.03.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class StatisticTracker : MonoBehaviour
    {
        public static StatisticTracker Instance { get; private set; }

        private int totalKills = 0;
        private float totalDealtDamage = 0;
        private float totalSufferedDamage = 0;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterKill()
        {
            totalKills++;
            Debug.Log($"Register Kill. Total Kills {totalKills}");
        }

        public void RegisterDealtDamage(float value)
        {
            totalDealtDamage += value;
            Debug.Log($"Register Dealt Damage. Total Damage {totalDealtDamage}");
        }

        public void RegisterSufferedDamage(float value)
        {
            totalSufferedDamage += value;
            Debug.Log($"Register Suffered Damage. Total Damage {totalSufferedDamage}");
        }
    }
}