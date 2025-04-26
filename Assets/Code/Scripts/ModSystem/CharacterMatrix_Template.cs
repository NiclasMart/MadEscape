using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatrixTemplate", menuName = "Scriptable Objects/CharacterMatrix/MatrixTemplate")]
public class CharacterMatrix_Template : ScriptableObject
{
    public string Name;
    public List<SkillRow> Rows = new();

    [Serializable]
    public class SkillRow
    {
        public MentalColorType RowColor;
        public List<MatrixSocketInfo> Slots;
    }

    public MatrixSocketInfo GetSocketInfo(int rowIndex, int columnIndex)
    {
        return Rows[rowIndex].Slots[columnIndex];
    }
}
