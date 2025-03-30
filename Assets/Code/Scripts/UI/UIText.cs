// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 30.03.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UIText : MonoBehaviour
    {
        [SerializeField] string format;
        private TextMeshProUGUI _display;

        void Awake()
        {
            _display = GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            SetTextDisplay("");
        }

        public void SetStatDisplay(float value)
        {
            SetTextDisplay(value.ToString("F2"));
        }

        public void SetStatDisplay(int value)
        {
            SetTextDisplay(value.ToString("F2"));
        }

        public void SetTextDisplay(string value)
        {
            _display.text = format.Replace("*", value);
        }
    }
}