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
        internal bool _active = false;

        public bool IsActive
        {
            get { return _active; }
        }

        public virtual void Initialize(CharacterStats stats)
        { }

        public void Activate()
        {
            _active = true;
        }

        public void Deactivate()
        {
            _active = false;
        }
    }
}