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

    List<List<Socket>> _matrix = new();

    public event Action OnUpdate_CharacterMatrix;

    public void Initialize()
    {
        if (_matrixDataReference == null) Debug.LogError("The character has no assigned Skillset!"); //Todo: durch assert austauschen

        for (int r = 0; r < _matrixDataReference.Rows.Count; r++)
        {
            _matrix.Add(new List<Socket>());
            for (int c = 0; c < _matrixDataReference.Rows[r].Slots.Count; c++)
            {
                CharacterMatrixTemplate.SocketInfo socketInfo = _matrixDataReference.GetSocketInfo(r, c);

                //create new sockets according to the predefined socket types
                switch (socketInfo.SocketType)
                {
                    case SocketType.Weapon:
                        _matrix[r].Add(new WeaponSocket(null, r, c));
                        break;
                    case SocketType.Mod:
                        _matrix[r].Add(new ModSocket(null, null, r, c));
                        break;
                    case SocketType.Mod | SocketType.Skill:
                        // TODO: improve logic
                        Type skillType = typeof(CharacterSkillLibrary).GetNestedType("DamageTaker");
                        Skill skill = gameObject.AddComponent(skillType) as Skill;
                        skill.Initialzize(socketInfo.SocketSkill.info, gameObject);
                        _matrix[r].Add(new ModSocket(null, skill, r, c));
                        break;
                    default:
                        throw new Exception($"Mod Type at position ({r},{c}) is not supported");
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
        ModSocket socket = _matrix[rowIndex][columnIndex] as ModSocket;
        if (socket != null && socket.Skill != null)
        {
            Action skill = socket.Skill.Unlock();
            if (skill == null) return;

            OnUpdate_CharacterMatrix += skill;
        }
    }
}

// --- Sockets ---

[Flags]
public enum SocketType
{
    None = 0,
    Weapon = 1,
    Mod = 2,
    Skill = 4,

}

public abstract class Socket
{
    int _rowIndex, _columnIndex;

    protected Socket(int rowIndex, int columnIndex)
    {
        _rowIndex = rowIndex;
        _columnIndex = columnIndex;
        Debug.Log($"Created Socket at position {rowIndex} : {columnIndex}");
    }

}

public class WeaponSocket : Socket
{
    Weapon _weapon;
    public WeaponSocket(Weapon weapon, int rowIndex, int columnIndex) : base(rowIndex, columnIndex)
    {
        _weapon = weapon;
    }
}

public class ModSocket : Socket
{
    Mod _mod;
    public Skill Skill { get; private set; }

    public ModSocket(Mod mod, Skill skill, int rowIndex, int columnIndex) : base(rowIndex, columnIndex)
    {
        _mod = mod;
        Skill = skill;
    }

    //returns previous mod if socket is not empty
    //else null
    public Mod SocketMod(Mod newMod)
    {
        Mod oldMod = _mod;
        _mod = newMod;
        return oldMod;
    }
}

public class Mod
{
    Stat _stat;
    float _value;

    public Mod(ModTemplate modTemplate)
    {
        _stat = modTemplate.ModifiedStat;
        _value = modTemplate.Value;
    }

}

//public interface ISocketable { }

public abstract class Skill : MonoBehaviour
{
    protected GameObject _target;
    private string _name;
    private bool _unlocked = false;
    private bool _needsUpdate; // defines if the skill is only used on unlock or acts like a passive over time

    public virtual void Initialzize(CharacterSkill.SkillInfo info, GameObject target)
    {
        _target = target;
        _name = info.Name;
        _needsUpdate = info.NeedsUpdateLoop;
    }

    // returns null if skill is instant use
    // returns the Action otherwise
    public Action Unlock()
    {
        _unlocked = true;
        if (!_needsUpdate)
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

// public class StatSkill : Skill
// {

// }

// public class PassiveSkill : Skill
// {

// }






