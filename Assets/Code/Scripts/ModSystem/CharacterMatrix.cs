using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Combat;
using NUnit.Framework;
using Stats;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterMatrix : MonoBehaviour
{
    [SerializeField] CharacterMatrixTemplate _matrixDataReference;

    List<List<ISocket<ISocketable>>> _matrix = new();

    public event Action OnUpdate_CharacterMatrix;

    public void Initialize()
    {
        Debug.Assert(_matrixDataReference != null, "The character has no assigned Skillset!");

        for (int r = 0; r < _matrixDataReference.Rows.Count; r++)
        {
            _matrix.Add(new List<ISocket<ISocketable>>());
            for (int c = 0; c < _matrixDataReference.Rows[r].Slots.Count; c++)
            {
                CharacterMatrixTemplate.SocketInfo socketInfo = _matrixDataReference.GetSocketInfo(r, c);

                //create new sockets according to the predefined socket types
                switch (socketInfo.SocketType)
                {
                    case SocketType.Weapon:
                        Socket<Weapon> weaponSocket = new Socket<Weapon>(r, c);
                        _matrix[r].Add(weaponSocket);
                        break;
                    case SocketType.Mod:
                    case SocketType.Mod | SocketType.Skill:
                        Socket<Mod> modSocket = new Socket<Mod>(r, c);
                        _matrix[r].Add(modSocket);
                        break;
                    default:
                        Debug.LogError($"Mod Type at position ({r},{c}) is not supported");
                        break;
                }
            }
        }

        UnlockSkill(0, 0);
    }

    void Update()
    {
        OnUpdate_CharacterMatrix?.Invoke(); // invoke all active skills
    }

    public void UnlockSkill(int rowIndex, int columnIndex)
    {
        ISocket<ISocketable> socket = _matrix[rowIndex][columnIndex];
        Debug.Assert(socket != null, $"ASSERT: The used socket at index {rowIndex} : {columnIndex} is not assigned.");

        OnUpdate_CharacterMatrix += socket?.Skill?.Unlock(); // registers skill to update loop
    }
}

// --- Sockets ---

[Flags]
public enum SocketType
{
    None = 0,
    Weapon = 1,
    Mod = 2,
    Skill = 4
}

public interface ISocketable { }

public interface ISocket<out T> where T : ISocketable
{
    T SocketedItem { get; }
    Skill Skill { get; }
}

public class Socket<T> : ISocket<T> where T : ISocketable
{
    int _rowIndex, _columnIndex;
    public T SocketedItem { get; private set; }
    public Skill Skill { get; private set; }

    public Socket(int rowIndex, int columnIndex, Skill skill = null)
    {
        _rowIndex = rowIndex;
        _columnIndex = columnIndex;
        Skill = skill;
        Debug.Log($"Created Socket at position {rowIndex} : {columnIndex}");
    }

    public T SocketItem(T newItem)
    {
        T oldSocketedItem = SocketedItem;
        SocketedItem = newItem;
        return oldSocketedItem;
    }

}

public class Mod : ISocketable
{
    Stat _stat;
    float _value;

    public Mod(ModTemplate modTemplate)
    {
        _stat = modTemplate.ModifiedStat;
        _value = modTemplate.Value;
    }

}



public abstract class Skill
{
    protected string _name;
    protected GameObject _user;
    [SerializeField] protected bool _unlocked = false;

    [Tooltip("Set true if one time activation on unlock. If continues update is required, set to false.")]
    protected bool _onlyActivatesOnUnlock; // defines if the skill is only used on unlock or acts like a passive over time

    public static Skill CreateSkillFromInfo(CharacterMatrixTemplate.SocketInfo socketInfo, GameObject target)
    {
        string skillRefName = socketInfo.SocketSkill.info.SkillRef;
        Type skillType = typeof(CharacterSkillLibrary).GetNestedType(skillRefName);
        Debug.Assert(skillType != null, $"ASSERT: Couldn't find the skill {skillRefName} in the Skill library.");

        Skill skill = Activator.CreateInstance(skillType, socketInfo.SocketSkill.info, target) as Skill;
        Debug.Assert(skill != null, $"ASSERT: Library skill {skillRefName} was of unexpected type. All Skills must inherit from Skill.");

        return skill;
    }

    public Skill(CharacterSkill.SkillInfo info, GameObject target)
    {
        _user = target;
        _name = info.Name;
        _onlyActivatesOnUnlock = info.OnlyActivatedOnceOnUnlock;
    }

    // returns null if skill is instant use
    // returns the Action otherwise
    public Action Unlock()
    {
        _unlocked = true;
        if (_onlyActivatesOnUnlock)
        {
            SkillEffect();
            return null;
        }
        else
        {
            return SkillEffect;
        }
    }

    protected abstract void SkillEffect(/*params object[] args*/);

}






