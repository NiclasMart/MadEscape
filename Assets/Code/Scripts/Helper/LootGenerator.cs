// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 15.10.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    //This class is not used in game and is only a depug tool
    public class LootGenerator : MonoBehaviour
    {
        [SerializeField] Pickup pickupPrefab;
        [SerializeField] List<Item> lootTable = new List<Item>();
        void Start()
        {
            //TODO: this implementation is only for testing the item
            Item item = lootTable[0];
            Pickup pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
            pickup.Generate(item);
            
        }
    }
}