using System;
using System.Reflection;
using UnityEngine;

namespace CharacterProgressionMatrix
{
    public abstract class Skill
    {
        public float Duration;
        public float Cooldown;
        private float _lastCastTime;
        private bool _active;
        
        protected string Name;
        protected GameObject User;

        [Tooltip("Set true if one time activation on unlock. If continues update is required, set to false.")]
        protected bool _onlyActivatesOnUnlock; // defines if the skill is only used on unlock or acts like a passive over time

        public static Skill CreateSkillFromTemplate(SkillTemplate.SkillInfo skillInfo, GameObject target)
        {
            string skillRefName = skillInfo.SkillRef;
            Type skillType = typeof(SkillLibrary).GetNestedType(skillRefName);
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
            User = target;
            Name = info?.Name;
            _lastCastTime = Time.time;
            _onlyActivatesOnUnlock = info?.OnlyActivatedOnceOnUnlock ?? false;
        }

        private void CastSkill()
        {
            if (_active)
            {
                SkillEffect();
                if (_lastCastTime + Duration <= Time.time) _active = false;
            }
            
            if (_lastCastTime + Duration + Cooldown < Time.time)
            {
                _active = true;
                _lastCastTime = Time.time;
            }
        }
        
        protected abstract void SkillEffect();

        // returns null if skill is instant use
        // returns the Action otherwise
        public Action RegisterSkill()
        {
            if (_onlyActivatesOnUnlock)
            {
                CastSkill();
                return null;
            }
            else
            {
                return CastSkill;
            }
        }
    }

    public abstract class ActiveSkill : Skill
    {
        protected ActiveSkill(SkillTemplate.SkillInfo info, GameObject target) : base(info, target) { }
    }
}