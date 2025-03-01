// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 08.10.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Items
{
    //this is a test
    public class ItemInventory : MonoBehaviour
    {
        [SerializeField] InventoryDisplay _display;
        Queue<ItemSlot> _slots = new();
        private readonly int _capacity = 5;
        GameObject _owner;

        public void Initialize(GameObject owner)
        {
            this._owner = owner;
            if (_display != null) _display.AddItems(new List<ItemSlot>(_slots));
        }

        //returns true if item was added to the inventory or used, false otherwise
        public bool Add(Item newItem)
        {
            //check if slot for item is free
            if (_slots.Count >= _capacity && !newItem.IsUsedInstantly) return false;

            newItem.transform.SetParent(transform);
            newItem.GetComponent<MeshRenderer>().enabled = false;

            if (newItem.IsUsedInstantly)
            {
                newItem.Use(_owner);
            }
            else
            {
                ItemSlot newSlot = new ItemSlot(newItem);
                _slots.Enqueue(newSlot);
                if (_display != null) _display.AddItem(newSlot);
            }
            return true;
        }

        public void UseItem(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Started || _slots.Count < 1) return;

            ItemSlot activatedSlot = _slots.Dequeue();
            activatedSlot.UseItem(_owner);
            if (_display != null) _display.RemoveItem();
        }
    }
}