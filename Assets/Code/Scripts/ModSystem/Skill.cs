using System;
using System.Reflection;
using UnityEngine;

namespace CharacterProgressionMatrix
{
    public abstract class Skill
    {
        protected string _name;
        protected GameObject _user;
        [SerializeField] protected bool _unlocked = false;

        [Tooltip("Set true if one time activation on unlock. If continues update is required, set to false.")]
        protected bool _onlyActivatesOnUnlock; // defines if the skill is only used on unlock or acts like a passive over time

        public static Skill CreateSkillFromTemplate(SkillTemplate.SkillInfo skillInfo, GameObject target)
        {
            string skillRefName = skillInfo.SkillRef;
            Type skillType = typeof(CharacterSkillLibrary).GetNestedType(skillRefName);
            Debug.Assert(skillType != null, $"ASSERT: Couldn't find the skill {skillRefName} in the Skill library.");


            Skill skill = Activator.CreateInstance(skillType, skillInfo, target) as Skill;
            Debug.Assert(skill != null, $"ASSERT: Library skill {skillRefName} was of unexpected type. All Skills must inherit from Skill.");

            FieldInfo[] parameterList = skillType.GetFields();
            for (int i = 0; i < parameterList.Length; i++)
            {

                parameterList[i].SetValue(skill, skillInfo.Parameters[i].ConvertValue<object>());
            }

            return skill;
        }

        protected Skill(SkillTemplate.SkillInfo info, GameObject target)
        {
            _user = target;
            _name = info?.Name;
            _onlyActivatesOnUnlock = info?.OnlyActivatedOnceOnUnlock ?? false;
        }
        
        protected abstract void SkillEffect();

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
    }
}