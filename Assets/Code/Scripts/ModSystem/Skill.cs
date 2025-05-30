using System;
using System.Reflection;
using UnityEngine;

namespace CharacterProgressionMatrix
{
    public abstract class Skill
    {
        public bool IsActive { get; private set; }
        
        protected GameObject User;
        
        private string _name;
        private float _duration;
        private float _cooldown;
        private bool _onlyActivatesOnUnlock; // defines if the skill is only used on unlock or acts like a passive over time
        private bool _alwaysActive;

        private float _lastCastTime;
        
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

        public Skill() { }

        protected Skill(SkillTemplate.SkillInfo info, GameObject target)
        {
            User = target;
            _name = info?.Name;
            _cooldown = info?.Cooldown ?? 0;
            _duration = info?.Duration ?? 0;
            _onlyActivatesOnUnlock = info?.OnlyActivatedOnceOnUnlock ?? false;
            _alwaysActive = info?.AlwaysActive ?? false;
            _lastCastTime = Time.time;
        }

        /// <summary>
        /// Registers the Skill and returns the skill action. If the skill is only one time activation, it is cast immediately and null is returned.
        /// </summary>
        /// <returns>The Skill Action if it is an active skill, null otherwise.</returns>
        public Func<bool> RegisterSkill()
        {
            if (_onlyActivatesOnUnlock)
            {
                StartSkillEffect();
                return null;
            }
            
            return UpdateSkillState;
        }
    
        /// <summary>
        /// Checks if the Skill is ready to use
        /// </summary>
        /// <returns>true if skill is ready, false otherwise</returns>
        public virtual bool ReadyToCast()
        {
            return (_lastCastTime + _duration + _cooldown < Time.time) || _alwaysActive;
        }
        
        public void Disable()
        {
            EndSkillEffect();
            IsActive = false;
        }
        
        public virtual void Reset()
        {
            IsActive = false;
            _lastCastTime = Time.deltaTime;
        }
        
        /// <summary>
        /// Is called after the Skill is created from a template, and should be used to initialize internal state that depends on public fields.
        /// </summary>
        protected abstract void InitializeAfterTemplateCreation();
        
        protected abstract void StartSkillEffect();
        
        protected abstract void SkillEffect();

        protected abstract void EndSkillEffect();
        
        private bool UpdateSkillState()
        {
            switch (IsActive)
            {
                // currently not active but ready to cast
                case false when ReadyToCast():
                    IsActive = true;
                    _lastCastTime = Time.time;
                    StartSkillEffect();
                    return true;
                // active and skill duration is reached,
                case true when !_alwaysActive && _lastCastTime + _duration <= Time.time:
                    EndSkillEffect();
                    IsActive = false;
                    return true;
                // active effect
                case true:
                    SkillEffect();
                    return true;
                // skill is inactive
                default:
                    return false;
            }
        }
    }
}