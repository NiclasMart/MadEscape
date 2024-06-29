// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 08.10.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Items
{
    public class ItemInventory : MonoBehaviour
    {
        [SerializeField] List<Item> activeSlots = new List<Item>();

        GameObject owner;

        public void Initialize(GameObject owner)
        {
            this.owner = owner;
        }

        public void Add(Item newItem)
        {
            newItem.transform.SetParent(transform);
            newItem.GetComponent<MeshRenderer>().enabled = false;

            if (newItem.isUsedInstantly)
            {
                newItem.Use(owner);
            }
            else
            {
                activeSlots.Add(newItem);
            }

        }
    }
}