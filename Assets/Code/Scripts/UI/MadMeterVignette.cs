// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 28.02.25
// Author: melonanas1@gmail.com
// Origin Project:
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using VitalForces;

namespace UI
{
    public class MadMeterVignette : MonoBehaviour
    {
        [SerializeField] private Sanity sanity; // Referenz zum Mad Meter (Sanity)
        [SerializeField] private Volume postProcessVolume; // Post-Processing Volume
        private Vignette vignette;

        private void Start()
        {
            if (postProcessVolume.profile.TryGet(out vignette))
            {
                vignette.intensity.Override(0f); // Startwert: Kein Effekt
            }
            else
            {
                Debug.LogError("Vignette konnte nicht gefunden werden! Ist der Effekt im Volume aktiviert?");
            }
        }

        private void Update()
        {
            if (sanity == null || vignette == null) return;

            float normalizedSanity = sanity.CurrentValue / sanity.MaxValue;
            vignette.intensity.Override(1f - normalizedSanity); // Je niedriger die Sanity, desto stärker der Effekt

            Debug.Log($"Sanity: {sanity.CurrentValue} / {sanity.MaxValue}, Normalized: {normalizedSanity}, Vignette: {vignette.intensity.value}");
        }
    }
}