using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/CharacterMatrix/Skill")]
public class SkillTemplate : ScriptableObject
{
    public SkillInfo info;

    [Serializable]
    public class SkillInfo
    {
        public string Name;
        public string SkillRef; //look into video: https://www.youtube.com/watch?v=jvokCXXYHCg
        public AnyValue[] Parameters;
        public bool OnlyActivatedOnceOnUnlock = false;
        public string Description;
        public bool isDirty = true;
    }
}

