// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 11.03.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class StatisticTracker : MonoBehaviour, IService
    {
        private int totalKills = 0;
        public UnityEvent<int> OnKillCountUpdate;
        private float totalDealtDamage = 0;
        public UnityEvent<float> OnDealtDamageUpdate;
        private float totalSufferedDamage = 0;
        public UnityEvent<float> OnSufferedDamageUpdate;

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