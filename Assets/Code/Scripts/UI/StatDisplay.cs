// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 20.12.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace UI
{
    public class StatDisplay : MonoBehaviour
    {
        [SerializeField] private CharacterStats connectedStats;
        Dictionary<Stat, StatText> slots = new Dictionary<Stat, StatText>();
        private void Awake()
        {
            BuildSlotDictionary();

            connectedStats.onStatsChanged += DisplayStat;
        }

        public void DisplayStat(Stat stat, float value)
        {
            if (!slots.ContainsKey(stat)) return;
            slots[stat].SetStatDisplay(value);
        }

        private void BuildSlotDictionary()
        {
            foreach (var slot in GetComponentsInChildren<StatText>(true))
            {
                if (slots.ContainsKey(slot.stat)) continue;
                slots.Add(slot.stat, slot);
            }
        }
    }
}