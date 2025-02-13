// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 25.11.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine;

namespace Items
{
    public class ItemSlot
    {
        Item _item = null;
        public bool HasItem => _item != null;

        public ItemSlot(Item item)
        {
            _item = item;
        }

        public Item GetItem()
        {
            return _item;
        }

        public void UseItem(GameObject user)
        {
            if (!HasItem) return;
            _item.Use(user);
        }
    }
}