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
        public bool IsActive { get; private set; }
        
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
            
            skill.InitializeAfterTemplateCreation();
            return skill;
        }
        
        /// <summary>
        /// Is called after the Skill is created from a template, and should be used to initialize internal state that depends on public fields.
        /// </summary>
        public abstract void InitializeAfterTemplateCreation();

        protected Skill(SkillTemplate.SkillInfo info, GameObject target)
        {
            User = target;
            Name = info?.Name;
            _lastCastTime = Time.time;
            _onlyActivatesOnUnlock = info?.OnlyActivatedOnceOnUnlock ?? false;
        }

        /// <summary>
        /// Registers the Skill and returns the skill action. If the skill is only one time activation, it is cast immediately and null is returned.
        /// </summary>
        /// <returns>The Skill Action if it is an active skill, null otherwise.</returns>
        public Action RegisterSkill()
        {
            if (_onlyActivatesOnUnlock)
            {
                SkillEffect();
                return null;
            }
            
            return UpdateSkillState;
            
        }
    
        /// <summary>
        /// Checks if the Skill is ready to use
        /// </summary>
        /// <returns>true if skill is ready, false otherwise</returns>
        public bool ReadyToCast()
        {
            return _lastCastTime + Duration + Cooldown < Time.time;
        }

        protected abstract void StartSkillEffect();
        
        protected abstract void SkillEffect();

        protected abstract void EndSkillEffect();
        
        private void UpdateSkillState()
        {
            switch (IsActive)
            {
                case true when _lastCastTime + Duration <= Time.time:
                    EndSkillEffect();
                    IsActive = false;
                    break;
                case true:
                    SkillEffect();
                    break;
                case false when ReadyToCast():
                    IsActive = true;
                    _lastCastTime = Time.time;
                    StartSkillEffect();
                    break;
            }
        }
    }

    // public abstract class ActiveSkill : Skill
    // {
    //     protected ActiveSkill(SkillTemplate.SkillInfo info, GameObject target) : base(info, target) { }
    // }
}