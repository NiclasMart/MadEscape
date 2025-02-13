// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 17.06.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using Stats;
using UnityEngine;

namespace EnemyActions
{
    public class Action : MonoBehaviour
    {
        internal bool active = false;

        public bool isActive
        {
            get { return active; }
        }

        public virtual void Initialize(CharacterStats stats)
        { }

        public void Activate()
        {
            active = true;
        }

        public void Deactivate()
        {
            active = false;
        }
    }
}