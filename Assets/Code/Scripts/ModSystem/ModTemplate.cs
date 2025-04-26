using Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "ModTemplate", menuName = "Scriptable Objects/CharacterMatrix/Mod")]
public class Mod_Template : ScriptableObject
{
    public Stat ModifiedStat;
    public float Value;
}
