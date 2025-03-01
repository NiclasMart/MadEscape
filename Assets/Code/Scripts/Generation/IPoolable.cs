// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 11.07.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using UnityEngine;

namespace Generation
{
    public interface IPoolable
    {
        event Action<IPoolable> OnDestroy;

        GameObject GetAttachedGameobject();
        void Reset();
    }
}