// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 08.10.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using Controller;
using Unity.VisualScripting;
using UnityEngine;

namespace Items
{
    public class Pickup : MonoBehaviour
    {
        private Item itemRef;

        public float collectRadius = 0.7f;

        public Item GetItem()
        {
            return itemRef;
        }

        public void Generate(Item item)
        {
            itemRef = Instantiate(item, transform);
        }

        private void OnTriggerStay(Collider other)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (!player) return;

            CollectItem(player);
        }

        private void CollectItem(PlayerController player)
        {
            if (!itemRef.CollectConditionIsFullfilled(player.gameObject)) return;

            player.Inventory.Add(itemRef);
            Destroy(gameObject);
        }
    }
}