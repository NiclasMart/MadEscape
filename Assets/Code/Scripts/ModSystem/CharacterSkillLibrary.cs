using UnityEngine;
using VitalForces;

public class CharacterSkillLibrary
{
    public class DamageTaker : Skill
    {
        private Health _health;

        public int Amount;

        public DamageTaker(CharacterSkill.SkillInfo info, GameObject user) : base(info, user)
        {
            _health = user.GetComponent<Health>();
        }

        protected override void SkillEffect(/*params object[] args*/)
        {
            _health.TakeDamage(Amount * Time.deltaTime);
            Debug.Log("Apply Damage");
        }
    }

    public class Healer : Skill
    {
        private Health _health;

        public int Amount;
        public float Multiplier;

        public Healer(CharacterSkill.SkillInfo info, GameObject user) : base(info, user)
        {
            _health = user.GetComponent<Health>();
        }

        protected override void SkillEffect(/*params object[] args*/)
        {
            _health.TakeDamage(Amount * Time.deltaTime);
            Debug.Log("Apply Damage");
        }
    }
}
