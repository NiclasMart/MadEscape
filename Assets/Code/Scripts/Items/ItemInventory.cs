// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 08.10.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Items
{
    //this is a test
    public class ItemInventory : MonoBehaviour
    {
        [SerializeField] InventoryDisplay display;
        Queue<ItemSlot> slots = new();
        private readonly int capacity = 5;
        GameObject owner;

        public void Initialize(GameObject owner)
        {
            this.owner = owner;
            if (display != null) display.AddItems(new List<ItemSlot>(slots));
        }

        //returns true if item was added to the inventory or used, false otherwise
        public bool Add(Item newItem)
        {
            //check if slot for item is free
            if (slots.Count >= capacity && !newItem.isUsedInstantly) return false;

            newItem.transform.SetParent(transform);
            newItem.GetComponent<MeshRenderer>().enabled = false;

            if (newItem.isUsedInstantly)
            {
                newItem.Use(owner);
            }
            else
            {
                ItemSlot newSlot = new ItemSlot(newItem);
                slots.Enqueue(newSlot);
                if (display != null) display.AddItem(newSlot, slots.Count - 1);
                Debug.Log($"Item {newItem.name} was stored in inventory.");
            }
            return true;
        }

        public void UseItem(InputAction.CallbackContext context)
        {   
            if (context.phase != InputActionPhase.Started || slots.Count < 1) return;

            ItemSlot activatedSlot = slots.Dequeue();
            activatedSlot.UseItem(owner);
            if (display != null) display.RemoveItem(activatedSlot);
        }
    }
}