// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 24.09.23
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    [System.Serializable]
    public class StatInputData
    {
        public Stat stat;
        public float value;
    }

    [CreateAssetMenu(fileName = "BaseStats", menuName = "MadEscape/BaseStats")]
    public class BaseStats : ScriptableObject
    {
        public List<StatInputData> inputStats = new List<StatInputData>();
    }
}