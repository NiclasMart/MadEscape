// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 25.11.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ItemSlot
    {
        Item item = null;
        public bool hasItem => item != null;

        public ItemSlot(Item item)
        {
            this.item = item;
        }

        public Item GetItem()
        {
            return item;
        }

        public void UseItem(GameObject user)
        {
            if (!hasItem) return;
            item.Use(user);
        }
    }
}