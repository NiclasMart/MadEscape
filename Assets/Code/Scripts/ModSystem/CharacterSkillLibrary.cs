using UnityEngine;
using VitalForces;

public class CharacterSkillLibrary
{
    public class DamageTaker : Skill
    {
        private Health _health;

        public DamageTaker(CharacterSkill.SkillInfo info, GameObject user) : base(info, user)
        {
            _health = user.GetComponent<Health>();
        }

        protected override void SkillEffect(/*params object[] args*/)
        {
            _health.TakeDamage(30 * Time.deltaTime);
            Debug.Log("Apply Damage");
        }
    }
}
