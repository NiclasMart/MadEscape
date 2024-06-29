// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 28.06.23
// Author: melonanas1@gmail.com
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceBar : MonoBehaviour
    {
        [SerializeField] private Slider bar;
        [SerializeField] private Gradient gradient;
        [SerializeField] private Image fill;

        [SerializeField] private TMP_Text uiText;

        int roundedMaxValue;
        int roundedCurrentValue;

        public void Initialize(float currentAmount, float maxAmount)
        {
            bar.maxValue = maxAmount;
            bar.value = currentAmount;

            fill.color = gradient.Evaluate(1f);
            if (uiText)
            {
                InitializeText(bar.maxValue);
            }
        }

        public void UpdateCurrentValue(float currentAmount)
        {
            bar.value = currentAmount;
            if(uiText)
            {
                UpdateCurrentText(currentAmount);
            }
            fill.color = gradient.Evaluate(bar.normalizedValue);
        }
        public void UpdateMaxValue(float newMaxValue)
        {
            bar.maxValue = newMaxValue;
        }
        public void InitializeText(float startValue)
        {   
            roundedMaxValue = (int)Math.Round(startValue, 0);
            float roundedCurrentValue = roundedMaxValue;
            uiText.text = roundedCurrentValue.ToString() + "/" + roundedMaxValue.ToString();
        }

        public void UpdateCurrentText(float currentValue)
        {
            roundedCurrentValue = (int)Math.Round(currentValue, 0);
            uiText.text = roundedCurrentValue.ToString() + "/" + roundedMaxValue.ToString();
        }
        public void ChangeFontStyle()
        {
            //we could change the font style and/or colour if our sanity is low
        }
    }
}