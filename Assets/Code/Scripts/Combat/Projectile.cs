// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 01.07.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Generation;
using VitalForces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        //Todo: delete hart coded stat and get it through player/weapon
        [SerializeField] private float speed = 5f;
        [SerializeField] private float lifetime = 5f;

        private float damage;
        private Vector3 movementVector;
        private float currentLifeSpan = 0;

        public event Action<IPoolable> onDestroy;

        private void FixedUpdate()
        {
            //move projectile forward
            transform.position += movementVector * Time.fixedDeltaTime;

            //check lifetime
            if (currentLifeSpan > lifetime)
            {
                Destroy();
            }
            currentLifeSpan += Time.fixedDeltaTime;
        }

        public void Fire(Vector3 direction, float damage)
        {
            this.damage = damage;
            movementVector = direction * speed;
            gameObject.SetActive(true);
        }

        public GameObject GetAttachedGameobject()
        {
            return gameObject;
        }

        private void Destroy()
        {
            currentLifeSpan = 0;
            onDestroy(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            Health target = other.GetComponent<Health>();
            if (target != null)
            {
                target.TakeDamage(damage);
                Debug.Log("Hit Enemy and dealt " + damage + " damage. Enemy Life: " + target.CurrentValue);
            }
            Destroy();
        }
    }
}