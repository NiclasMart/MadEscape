// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 21.05.25
// Author: melonanas1@gmail.com
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using VitalForces;

namespace AI
{
    public class StalkerBehaviour : AgentBehaviour
    {
         private Transform _player;
        private float _timeSinceLastDash = 0f;
        private Vector3 _dashTargetPosition;
        private bool _isDashing = false;

        // Konfiguration
        [SerializeField] private float minDistance = 5f;
        [SerializeField] private float maxDistance = 15f;
        [SerializeField] private float dashInterval = 10f;
        [SerializeField] private float dashDuration = 2f;
        [SerializeField] private Sanity sanity;
        void Start()
        {
            _player = ServiceProvider.Get<GameManager>().GetPlayer().transform;
            if (_player == null)
                Debug.LogError("Player-Transform ist nicht gesetzt!");

            sanity = _player.GetComponent<Sanity>();

            if (sanity == null)
                Debug.LogError("Sanity-Referenz fehlt!");

        }
        protected override Vector3 CalculateNewTargetPosition()
        {
            float normalizedSanity = sanity.CurrentValue / sanity.MaxValue;
            float desiredDistance = Mathf.Lerp(minDistance, maxDistance, 1f - normalizedSanity);

            Vector3 directionToPlayer = (_player.position - transform.position).normalized;
            float currentDistance = Vector3.Distance(transform.position, _player.position);

            _timeSinceLastDash += Time.deltaTime;

            /* if (_isDashing)
            {
                if (_timeSinceLastDash > dashDuration)
                {
                    _isDashing = false;
                    _timeSinceLastDash = 0f;
                }
                return _dashTargetPosition;
            }

            if (_timeSinceLastDash >= dashInterval)
            {
                _isDashing = true;
                _dashTargetPosition = _player.position;
                _timeSinceLastDash = 0f;
                return _dashTargetPosition;
            } */

            // Abstand halten
            if (Mathf.Abs(currentDistance - desiredDistance) > 1f)
            {
                return _player.position - directionToPlayer * desiredDistance;
            }

            return transform.position;
        }
    }
}