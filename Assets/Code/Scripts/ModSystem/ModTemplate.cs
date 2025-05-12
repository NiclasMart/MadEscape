using Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "ModTemplate", menuName = "Scriptable Objects/CharacterMatrix/Mod")]
public class ModTemplate : ScriptableObject
{
    public Stat ModifiedStat;
    public float Value;
}
