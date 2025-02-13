// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 08.10.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Controller;
using UnityEngine;

namespace Items
{
    public class Pickup : MonoBehaviour
    {
        private Item _itemRef;

        public readonly float CollectRadius = 0.7f;

        public Item GetItem()
        {
            return _itemRef;
        }

        public void Generate(Item item)
        {
            _itemRef = Instantiate(item, transform);
        }

        private void OnTriggerStay(Collider other)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (!player) return;

            CollectItem(player);
        }

        private void CollectItem(PlayerController player)
        {
            if (!_itemRef.CollectConditionIsFullfilled(player.gameObject)) return;

            bool itemWasConsumed = player.Inventory.Add(_itemRef);
            if (itemWasConsumed) Destroy(gameObject);
        }
    }
}