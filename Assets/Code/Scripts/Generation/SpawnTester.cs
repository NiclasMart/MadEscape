using System;
using System.Collections;
using Controller;
using Generation;
using UnityEngine;

namespace Generator
{
    public class SpawnTester : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _spawnDelay;

        private bool _canSpawn = true;

        public event Action<IPoolable> OnDestroy;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerController>() != null)
            {
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

        public IEnumerator SpawningEnemy(int enemyTypeID, Action<Vector3, int> callback)
        {
            yield return new WaitForSeconds(_spawnDelay);
            if (_canSpawn) callback?.Invoke(transform.position, enemyTypeID);
            OnDestroy?.Invoke(this);
        }

        public GameObject GetAttachedGameobject()
        {
            return gameObject;
        }

        public void Reset()
        {
            _canSpawn = true;
        }
    }
}
