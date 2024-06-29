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

namespace VitalForces
{
    public class Sanity : Resource
    {
        float sanityDecAmount;
        [SerializeField] public int convertedHealthAmount = 100;
        private float sanityConversionFactor;

        private Health playerHealth;

        private float timer = 0f;
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
            stats.onStatsChanged += UpdateSanityStat;
            Initialize(sanity, sanity);
        }

        private void Update()
        {
            if (timer > 1)
            {
                ChangeSanity(-sanityDecAmount);
                timer = 0f;
            }
            timer += Time.deltaTime;
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
                ChangeSanity(convertedHealthAmount * sanityConversionFactor);
            }
        }
        private void UpdateSanityStat(Stat stat, float newValue)
        {   
            if(stat != Stat.Sanity) return;
            sanity = newValue;
            UpdateDisplay(sanity);
        }
    }
}