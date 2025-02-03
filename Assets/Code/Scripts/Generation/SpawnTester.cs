using System;
using System.Collections;
using Controller;
using UnityEngine;

namespace Generator
{
    public class SpawnTester : MonoBehaviour
    {
        [SerializeField] private float _spawnDelay;

        private bool _canSpawn = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                Debug.Log("Disable spawn");
                _canSpawn = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                _canSpawn = true;
            }
        }

        public IEnumerator SpawningEnemy(Vector3 position, Action<Vector3> callback)
        {
            yield return new WaitForSeconds(_spawnDelay);
            if (_canSpawn) callback?.Invoke(position);
            Destroy(gameObject);
        }
    }
}
