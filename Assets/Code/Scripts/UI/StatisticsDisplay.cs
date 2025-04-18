// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 18.04.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class StatisticsDisplay : MonoBehaviour
    {
        [SerializeField] private StatisticsRecord _record;
        [SerializeField] private UIText _killCount, _dealtDamage, _sufferedDamage;

        void OnEnable()
        {
            _record.OnKillCountUpdate.AddListener(value => _killCount.SetValueDisplay(value));
            _record.OnDealtDamageUpdate.AddListener(value => _dealtDamage.SetValueDisplay(value));
            _record.OnSufferedDamageUpdate.AddListener(value => _sufferedDamage.SetValueDisplay(value));
        }

        void OnDisable()
        {
            _record.OnKillCountUpdate.RemoveListener(value => _killCount.SetValueDisplay(value));
            _record.OnDealtDamageUpdate.RemoveListener(value => _dealtDamage.SetValueDisplay(value));
            _record.OnSufferedDamageUpdate.RemoveListener(value => _sufferedDamage.SetValueDisplay(value));
        }

        void Start()
        {
            ServiceProvider.Get<StatisticTracker>();
        }
    }
}