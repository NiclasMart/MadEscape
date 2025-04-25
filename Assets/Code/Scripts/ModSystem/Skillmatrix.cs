using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSet", menuName = "Scriptable Objects/Skills/SkillSet")]
public class SkillSet : ScriptableObject
{
    public string Name;
    public List<SkillRow> Rows = new();

    [Serializable]
    public class SkillRow
    {
        public MentalColorType RowColor;
        public List<SkillInfo> Skills;
    }

    public SkillInfo GetSkillInfo(int rowIndex, int columnIndex)
    {
        return Rows[rowIndex].Skills[columnIndex];
    }
}

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skills/Skill")]
public class SkillInfo : ScriptableObject
{
    public string Name;
    public Action Ability; //look into video: https://www.youtube.com/watch?v=jvokCXXYHCg
    public bool RandomizeColor = false;
    [TextArea(1, 5)] public string Description;
}


public class ModGrid : MonoBehaviour
{
    [SerializeField] SkillSet _skillSetRef;

    List<List<Socket>> _skills = new();

    void Awake()
    {
        if (_skillSetRef == null) Debug.LogError("The character has no assigned Skillset!"); //Todo: durch assert austauschen

        for (int r = 0; r < _skillSetRef.Rows.Count; r++)
        {
            _skills.Add(new List<Socket>());
            for (int c = 0; c < _skillSetRef.Rows[r].Skills.Count; c++)
            {
                SkillInfo skillInfo = _skillSetRef.GetSkillInfo(r, c);
                _skills[r].Add(new Socket(r, c));
            }
        }
    }

    void Update()
    {

    }

    public void UnlockSkill(int rowIndex, int columnIndex)
    {
       
    }
}

public class Socket 
{
    int _rowIndex, _columnIndex;

    public Socket(int rowIndex, int columnIndex)
    {
        _rowIndex = rowIndex;
        _columnIndex = columnIndex;

        Debug.Log($"Created Socket at position {rowIndex} : {columnIndex}");
    }

}

public interface ISocketable {}

public class Skill : ISocketable
{
    private string _name;
    private Action _ability; //look into video: https://www.youtube.com/watch?v=jvokCXXYHCg
    private bool _unlocked = false;

    public Skill(SkillInfo skillInfo)
    {
        _name = skillInfo.Name;
        _ability = skillInfo.Ability;
    }

    public void Unlock()
    {
        //Todo: do registration logic

        _unlocked = true;
    }
}

// public class StatSkill : Skill
// {

// }

// public class PassiveSkill : Skill
// {

// }






