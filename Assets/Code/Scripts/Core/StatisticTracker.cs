// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 11.03.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine;

namespace Core
{
    public class StatisticTracker : MonoBehaviour, IService
    {
        [SerializeField] private StatisticsRecord _record;

        public void RegisterKill()
        {
            _record.AddKill();
        }

        public void RegisterDealtDamage(float value)
        {
            _record.AddDealtDamage(value);
        }

        public void RegisterSufferedDamage(float value)
        {
            _record.AddSufferedDamage(value);
        }

        public void ResetStatistics()
        {
            _record.Reset();
        }
    }
}