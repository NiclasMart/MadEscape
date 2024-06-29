// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 28.06.23
// Author: melonanas1@gmail.com
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceBar : MonoBehaviour
    {
        [SerializeField] private Slider bar;
        [SerializeField] private Gradient gradient;
        [SerializeField] private Image fill;

        public void Initialize(float currentAmount, float maxAmount)
        {
            bar.maxValue = maxAmount;
            bar.value = currentAmount;

            fill.color = gradient.Evaluate(1f);
        }

        public void UpdateCurrentValue(float currentAmount)
        {
            bar.value = currentAmount;

            fill.color = gradient.Evaluate(bar.normalizedValue);
        }
        public void UpdateMaxValue(float newMaxValue)
        {
            bar.maxValue = newMaxValue;
        }
    }
}