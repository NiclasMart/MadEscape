using UnityEngine;
using VitalForces;

public class CharacterSkillLibrary
{
    public class DamageTaker : Skill
    {
        private Health _health;

        public int Amount = 20;
        public Vector3 Direction = Vector3.forward;

        public DamageTaker(CharacterSkill.SkillInfo info, GameObject user) : base(info, user)
        {
            if (user == null) return;
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

        public static int Amount = 5;
        public static float Multiplier = 10;

        public Healer(CharacterSkill.SkillInfo info, GameObject user) : base(info, user)
        {
            if (user == null) return;
            _health = user.GetComponent<Health>();
        }

        protected override void SkillEffect(/*params object[] args*/)
        {
            _health.TakeDamage(Amount * Time.deltaTime);
            Debug.Log("Apply Damage");
        }
    }

    public class Test : Skill
    {
        public Test(CharacterSkill.SkillInfo info, GameObject target) : base(info, target)
        {
        }

        protected override void SkillEffect()
        {

        }
    }
}
