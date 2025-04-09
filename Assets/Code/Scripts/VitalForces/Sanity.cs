// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 10.08.23
// Author: anonymous
// Origin Project:
// ---------------------------------------------
// -------------------------------------------*/
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace VitalForces
{
    public class Sanity : Resource
    {
        float sanityDecAmount;
        [SerializeField] public int convertedHealthAmount = 100;
        [SerializeField] public float sanityRestoreTime = 0.5f; // Time to restore sanity over time
        private float sanityConversionFactor;

        private Health playerHealth;

        float sanity;

        private void Start()
        {
            playerHealth = GetComponent<Health>();
        }

        public void Initialize(CharacterStats stats)
        {
            sanity = stats.GetStat(Stat.Sanity);
            sanityDecAmount = stats.GetStat(Stat.SanityDecAmount);
            sanityConversionFactor = stats.GetStat(Stat.SanityConversionFactor);
            stats.OnStatsChanged += UpdateSanityStat;
            Initialize(sanity, sanity);
        }

        private void Update()
        {
            ChangeSanity(-sanityDecAmount * Time.deltaTime);
        }

        public void ChangeSanity(float sanityChangeAmount)
        {
            Change(sanityChangeAmount);
        }

        public void SanityConversion(InputAction.CallbackContext context)
        {
            if (context.performed && (playerHealth.CurrentValue > convertedHealthAmount))
            {
                playerHealth.TakeDamage(convertedHealthAmount);
                float sanityGain = convertedHealthAmount * sanityConversionFactor;
                StartCoroutine(RestoreSanityOverTime(sanityGain, sanityRestoreTime));
            }
        }
        private IEnumerator RestoreSanityOverTime(float totalAmount, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float delta = (totalAmount / duration) * Time.deltaTime;
                ChangeSanity(delta);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }       
        private void UpdateSanityStat(Stat stat, float newValue)
        {
            if (stat != Stat.Sanity && stat != Stat.SanityDecAmount && stat != Stat.SanityConversionFactor) return;
            if (stat == Stat.Sanity)
            {
                sanity = newValue;
                UpdateDisplay(sanity);
            }
            if (stat == Stat.SanityDecAmount) sanityDecAmount = newValue;
            if (stat == Stat.SanityConversionFactor) sanityConversionFactor = newValue;
        }
    }
}