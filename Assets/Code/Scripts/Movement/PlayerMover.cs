// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 16.06.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

namespace Movement
{
    public class PlayerMover : MonoBehaviour
    {
        private float movementSpeed;

        private Vector2 movementVector = Vector2.zero;

        private Rigidbody rigidBody;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        public void Initialize(CharacterStats stats)
        {
            movementSpeed = stats.GetStat(Stat.MovementSpeed);
            stats.onStatsChanged += UpdateMovementspeed;
            rigidBody.sleepThreshold = 0;
        }

        private void FixedUpdate()
        {
            rigidBody.AddForce(new Vector3(movementVector.x, 0, movementVector.y) * movementSpeed, ForceMode.Force);
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                movementVector = context.ReadValue<Vector2>();
            }
            else if (context.canceled)
            {
                movementVector = Vector2.zero;
            }
        }

        private void UpdateMovementspeed(Stat stat, float newValue)
        {
            if (stat != Stat.MovementSpeed) return;
            movementSpeed = newValue;
        }
    }
}