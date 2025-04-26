using System;
using System.Collections.Generic;
using Combat;
using NUnit.Framework;
using Stats;
using UnityEngine;


public class CharacterMatrix : MonoBehaviour
{
    [SerializeField] CharacterMatrix_Template _matrixDataReference;

    List<List<Socket>> _matrix = new();

    public event Action OnUpdate;

    void Awake()
    {
        if (_matrixDataReference == null) Debug.LogError("The character has no assigned Skillset!"); //Todo: durch assert austauschen

        for (int r = 0; r < _matrixDataReference.Rows.Count; r++)
        {
            _matrix.Add(new List<Socket>());
            for (int c = 0; c < _matrixDataReference.Rows[r].Slots.Count; c++)
            {
                MatrixSocketInfo socketInfo = _matrixDataReference.GetSocketInfo(r, c);

                switch (socketInfo.SocketType)
                {
                    case SocketType.Weapon:
                        _matrix[r].Add(new WeaponSocket(null, r, c));
                        break;
                    case SocketType.Mod:
                        _matrix[r].Add(new ModSocket(null, null, r, c));
                        break;
                    case SocketType.Mod | SocketType.Skill:
                        Skill skill = new Skill(socketInfo);
                        _matrix[r].Add(new ModSocket(null, skill, r, c));
                        break;
                    default:
                        throw new Exception($"Mod Type at position ({r},{c}) is not supported");
                }
            }
        }

        Debug.Log("Created matrix.");
    }

    void Update()
    {
        OnUpdate.Invoke();
    }

    public void UnlockSkill(int rowIndex, int columnIndex)
    {

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

public class Socket
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
    Skill _skill = null;

    public ModSocket(Mod mod, Skill skill, int rowIndex, int columnIndex) : base(rowIndex, columnIndex)
    {
        _mod = mod;
        _skill = skill;
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

    public Mod(Mod_Template modTemplate)
    {
        _stat = modTemplate.ModifiedStat;
        _value = modTemplate.Value;
    }

}

//public interface ISocketable { }

public class Skill
{
    private string _name;
    private Action _ability; //look into video: https://www.youtube.com/watch?v=jvokCXXYHCg
    private bool _unlocked = false;

    public Skill(MatrixSocketInfo skillInfo)
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






