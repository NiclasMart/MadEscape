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
using TMPro;
using UnityEngine;

namespace UI
{
    public class StatText : MonoBehaviour
    {
        public Stat stat;
        [SerializeField] string format;

        public void SetStatDisplay(float value)
        {
            GetComponent<TextMeshProUGUI>().text = format.Replace("*", value.ToString("F2"));
        }

    }
}