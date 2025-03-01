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
        [SerializeField] private GameObject _poolType;
        [SerializeField] private Transform _poolHolder;

        private readonly List<IPoolable> _disabledObjects = new();

        public void Initialize(GameObject poolType, int initialCapacity, Transform poolHolder)
        {
            _poolType = poolType;
            _initialCapacity = initialCapacity;
            _poolHolder = poolHolder;

            //initialize pool according to initial capacity
            for (int i = 0; i < _initialCapacity; i++)
            {
                _disabledObjects.Add(CreateNewObject());
            }
        }

        public GameObject GetObject()
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
            return objectInstance.GetAttachedGameobject();
        }

        public void ReturnObject(IPoolable returnedObject)
        {
            returnedObject.Reset();
            returnedObject.GetAttachedGameobject().SetActive(false);
            _disabledObjects.Add(returnedObject);
        }

        private IPoolable CreateNewObject()
        {
            GameObject newObject = Instantiate(_poolType, _poolHolder);
            newObject.SetActive(false);

            IPoolable poolable = newObject.GetComponent<IPoolable>();
            if (poolable == null) Debug.LogError("Pooled Object isn't of type IPoolable. Add the interface to make it poolable.");
            poolable.OnDestroy += ReturnObject;

            return poolable;
        }
    }
}