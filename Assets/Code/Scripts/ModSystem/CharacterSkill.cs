using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/CharacterMatrix/Skill")]
public class CharacterSkill : ScriptableObject
{
    public SkillInfo info;

    [Serializable]
    public class SkillInfo
    {
        public string Name;
        public string SkillRef; //look into video: https://www.youtube.com/watch?v=jvokCXXYHCg
        public bool NeedsUpdateLoop = false;
        public string Description;
    }

}

//implement as custom property drawer in CharacterMatrix_Template: https://www.youtube.com/watch?v=3w9iXT6wx5g
