// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 25.06.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Generation;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private int _initialCapacity = 10;
        public int InitialCapacity => _initialCapacity;
        [SerializeField] private int _maxCapacity = 50;
        public int MaxCapacity => _maxCapacity;
        [SerializeField] private GameObject _poolType;
        [SerializeField] private Transform _poolHolder;
        private readonly List<IPoolable> _disabledObjects = new();
        public int CurrentCapacity { get; private set; }
        public int AvailableSpace => _disabledObjects.Count;

        public void Initialize(GameObject poolType, Transform poolHolder, int initialCapacity = 10, int maxCapacity = 50)
        {
            _poolType = poolType;
            _initialCapacity = initialCapacity;
            _poolHolder = poolHolder;
            _maxCapacity = maxCapacity;

            Initialize();
        }

        public void Initialize()
        {
            //initialize pool according to initial capacity
            for (int i = 0; i < _initialCapacity; i++)
            {
                _disabledObjects.Add(CreateNewObject());
            }
        }

        public bool TryGetObject(out GameObject pooledObject)
        {
            IPoolable objectInstance;
            if (_disabledObjects.Count == 0)
            {
                objectInstance = CreateNewObject();
            }
            else
            {
                objectInstance = _disabledObjects[_disabledObjects.Count - 1];
                _disabledObjects.RemoveAt(_disabledObjects.Count - 1);
            }

            pooledObject = objectInstance?.GetAttachedGameobject();
            return pooledObject != null;
        }

        public void ReturnObject(IPoolable returnedObject)
        {
            returnedObject.Reset();
            returnedObject.GetAttachedGameobject().SetActive(false);
            _disabledObjects.Add(returnedObject);
        }

        private IPoolable CreateNewObject()
        {
            // check max capacity
            if (CurrentCapacity >= _maxCapacity)
            {
                Debug.LogWarning($"Pool {gameObject.name} has reached his Max-Capacity of {_maxCapacity}. New elemet is not generated.");
                return null;
            }

            // generate new object
            GameObject newObject = Instantiate(_poolType, _poolHolder);
            newObject.SetActive(false);
            if (!newObject.TryGetComponent<IPoolable>(out var poolable))
            {
                Debug.LogError($"Pooled Object {newObject.name} isn't of type IPoolable. Add the interface to make it poolable.");
            }
            CurrentCapacity++;

            poolable.OnDestroy += ReturnObject;
            return poolable;
        }
    }
}