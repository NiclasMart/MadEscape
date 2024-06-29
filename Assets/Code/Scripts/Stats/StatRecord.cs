// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 26/11/23
// Author: langner.paul@t-online.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections.Generic;

namespace Stats
{
    [System.Serializable]
    public class StatRecord
    {
        public int id;
        public string name;
        public Dictionary<Stat, float> statDict;
    }
}