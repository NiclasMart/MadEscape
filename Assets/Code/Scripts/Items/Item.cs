// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 08.10.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public abstract class Item : MonoBehaviour
    {
        [SerializeField] new string name;
        [SerializeField] bool instantUse;
        public bool isUsedInstantly => instantUse;
        //icon

        public abstract void Use(GameObject user);

        //this can define a condition, under which the can be picked up by the player
        public abstract bool CollectConditionIsFullfilled(GameObject user);
    }
}