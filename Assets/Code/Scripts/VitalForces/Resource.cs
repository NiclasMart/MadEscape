// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 10.07.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace VitalForces
{
    public class Resource : MonoBehaviour
    {
        [SerializeField] private ResourceBar connectedDisplay;

        private float maxValue;
        private float currentValue;
        public float CurrentValue => currentValue;
        public float ProportionalValue => currentValue / maxValue;
        public float MaxValue => maxValue;

        protected void Initialize(float currentValue, float maxValue)
        {
            this.currentValue = currentValue;
            this.maxValue = maxValue;
            InitializeDisplay();
        }

        //adds the value to the currentValue of the resource, value can be positive or negative
        public void Change(float value)
        {
            currentValue = Mathf.Clamp(currentValue + value, 0, maxValue);
            connectedDisplay?.UpdateCurrentValue(currentValue);
        }

        private void InitializeDisplay()
        {
            if (connectedDisplay == null) return;

            connectedDisplay.Initialize(CurrentValue, maxValue);
        }

        public void UpdateDisplay(float newMaxValue)
        {
            if (connectedDisplay == null) return;

            connectedDisplay.UpdateMaxValue(newMaxValue);
        }
    }
}