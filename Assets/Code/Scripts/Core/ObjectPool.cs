// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 25.06.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Generation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private int initialCapacity = 10;
        [SerializeField] private GameObject poolType;
        [SerializeField] private Transform poolHolder;

        private List<IPoolable> disabledObjects = new List<IPoolable>();

        private void Awake()
        {
            if (poolHolder == null)
            {
                poolHolder = new GameObject(transform.name).transform;
            }
        }

        private void Start()
        {
            //initialize pool according to initial capacity
            for (int i = 0; i < initialCapacity; i++)
            {
                disabledObjects.Add(CreateNewObject());
            }
        }

        public GameObject GetObject()
        {
            IPoolable objectInstance;
            if (disabledObjects.Count == 0)
            {
                objectInstance = CreateNewObject();
            }
            else
            {
                objectInstance = disabledObjects[disabledObjects.Count - 1];
                disabledObjects.RemoveAt(disabledObjects.Count - 1);
            }
            return objectInstance.GetAttachedGameobject();
        }

        public void ReturnObject(IPoolable returnedObject)
        {
            returnedObject.GetAttachedGameobject().SetActive(false);
            disabledObjects.Add(returnedObject);
        }

        private IPoolable CreateNewObject()
        {
            GameObject newObject = Instantiate(poolType, poolHolder);
            newObject.SetActive(false);

            IPoolable poolable = newObject.GetComponent<IPoolable>();
            if (poolable == null) Debug.LogError("Pooled Object isn't of type IPoolable. Add the interface to make it poolable.");
            poolable.OnDestroy += ReturnObject;

            return poolable;
        }
    }
}