using System;
using System.Collections.Generic;
using CharacterProgressionMatrix;
using UnityEngine;

namespace CharacterProgressionMatrix
{
    [CreateAssetMenu(fileName = "MatrixTemplate", menuName = "Scriptable Objects/CharacterMatrix/MatrixTemplate")]
    public class CharacterMatrixTemplate : ScriptableObject
    {
        public string Name;
        public List<MatrixRow> Rows = new();

        [Serializable]
        public class MatrixRow
        {
            public MentalColorType RowColor;
            public List<SocketInfo> Slots;
        }

        [Serializable]
        public class SocketInfo
        {
            public SocketType SocketType;
            public SkillTemplate SocketSkillTemplate;
        }

        public SocketInfo GetSocketInfo(int rowIndex, int columnIndex)
        {
            return Rows[rowIndex].Slots[columnIndex];
        }
    }
}
