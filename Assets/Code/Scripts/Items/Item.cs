// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 08.10.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine;

namespace Items
{
    public abstract class Item : MonoBehaviour
    {
        [SerializeField] string _name;
        [SerializeField] bool _instantUse;
        public Color DebugColor;
        public bool IsUsedInstantly => _instantUse;
        //icon

        public abstract void Use(GameObject user);

        //this can define a condition, under which the can be picked up by the player
        public abstract bool CollectConditionIsFullfilled(GameObject user);
    }
}