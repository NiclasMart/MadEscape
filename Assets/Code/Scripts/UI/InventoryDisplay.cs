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
        int itemAmount = 0;

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
            itemAmount = items.Count;
            for (int i = 0; i < items.Count; i++)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].color = items[i].GetItem().debugColor;
            }
        }

        public void AddItem(ItemSlot item)
        {
            slots[itemAmount].gameObject.SetActive(true);
            slots[itemAmount].color = item.GetItem().debugColor;
            itemAmount++;
        }

        public void RemoveItem()
        {
            if (itemAmount <= 1) slots[0].gameObject.SetActive(false);
            for (int i = 0; i < itemAmount; i++)
            {
                int nextIndex = i + 1;
                slots[i].color = slots[nextIndex].color;

                //if next item is the last one, disable display for it and exit
                if (nextIndex == itemAmount - 1) 
                {
                    slots[nextIndex].gameObject.SetActive(false);
                    itemAmount--;
                    break;
                }
            }
        }
    }
}