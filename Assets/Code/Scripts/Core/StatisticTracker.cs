// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 11.03.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class StatisticTracker : MonoBehaviour
    {
        private int totalKills = 0;
        public UnityEvent<int> OnKillCountUpdate;
        private float totalDealtDamage = 0;
        public UnityEvent<float> OnDealtDamageUpdate;
        private float totalSufferedDamage = 0;
        public UnityEvent<float> OnSufferedDamageUpdate;

        void Awake()
        {
            if (ServiceProvider.Get<StatisticTracker>() != null)
            {
                Destroy(gameObject);
                return;
            }

            ServiceProvider.Register(this);
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterKill()
        {
            totalKills++;
            OnKillCountUpdate?.Invoke(totalKills);
        }

        public void RegisterDealtDamage(float value)
        {
            totalDealtDamage += value;
            OnDealtDamageUpdate?.Invoke(totalDealtDamage);
        }

        public void RegisterSufferedDamage(float value)
        {
            totalSufferedDamage += value;
            OnSufferedDamageUpdate?.Invoke(totalSufferedDamage);
        }
    }
}