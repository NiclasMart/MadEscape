// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 20.12.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using Combat;
using Stats;
using UnityEngine;

public enum StatSource
{
    Character,
    Weapon
}

namespace UI
{
    public class StatDisplay : MonoBehaviour
    {
        [SerializeField] private CharacterStats _connectedCharacterStats;
        [SerializeField] private WeaponController _connectedWeaponController;
        Dictionary<Stat, StatText> _slots = new Dictionary<Stat, StatText>();

        private void Awake()
        {
            BuildSlotDictionary();

            _connectedCharacterStats.OnStatsChanged += DisplayStat;
            _connectedWeaponController.OnStatChanged += DisplayStat;
        }

        public void DisplayStat(Stat stat, float value)
        {
            if (!_slots.ContainsKey(stat)) return;

            _slots[stat].SetValueDisplay(value);
        }

        private void BuildSlotDictionary()
        {
            foreach (var slot in GetComponentsInChildren<StatText>(true))
            {
                if (_slots.ContainsKey(slot.Stat)) continue;
                _slots.Add(slot.Stat, slot);
            }
        }
    }
}