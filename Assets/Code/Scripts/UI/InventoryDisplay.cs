// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 07.07.24
// Author: niclas.mart@telekom.de
// Origin Project: 
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryDisplay : MonoBehaviour
    {
        List<Image> slots = new List<Image>();

        private void Awake()
        {
            //get slot references
            foreach (Transform slot in transform)
            {
                slots.Add(slot.GetComponent<Image>());
                slot.gameObject.SetActive(false);
            }
        }

        public void AddItems(List<ItemSlot> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                //TODO: assign image
                slots[i].gameObject.SetActive(true);
                slots[i].color = items[i].GetItem().debugColor;
            }
        }

        public void AddItem(ItemSlot item, int position)
        {
            Debug.Log("Add item " + item.GetItem().name);
            slots[position].gameObject.SetActive(true);
            slots[position].color = item.GetItem().debugColor;
        }

        public void RemoveItem(ItemSlot item)
        {
            //reorder items in display
        }
    }
}