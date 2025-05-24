using UnityEngine;
using VitalForces;

namespace CharacterProgressionMatrix
{
    public class SkillLibrary
    {
        public class Healer : Skill
        {
            private Health _health;

            public int Amount = 5;
            public float Multiplier = 10;
            public GameObject _objectRef;

            public Healer(SkillTemplate.SkillInfo info, GameObject user) : base(info, user)
            {
                if (user == null) return;
                _health = user.GetComponent<Health>();
            }

            protected override void SkillEffect( /*params object[] args*/)
            {
                _health.TakeDamage(Amount * Time.deltaTime);
                Debug.Log("Apply Damage");
            }
        }

        public class DamageArea : ActiveSkill
        {
            public float Size;
            
            public DamageArea(SkillTemplate.SkillInfo info, GameObject target) : base(info, target)
            {
                
            }

            protected override void SkillEffect()
            {
                Debug.Log($"Activate Area {User.gameObject.GetInstanceID()}");
            }
        }
    }
}
