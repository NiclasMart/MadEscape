using UnityEngine;
using VitalForces;

public class CharacterSkillLibrary
{
    public static void TestSkill(GameObject target)
    {
        target.GetComponent<Health>().TakeDamage(30 * Time.deltaTime);
    }

    public class DamageTaker : Skill
    {
        private Health _health;

        public override void Initialzize(CharacterSkill.SkillInfo info, GameObject target)
        {
            base.Initialzize(info, target);
            _health = target.GetComponent<Health>();
        }

        protected override void SkillEffect(/*params object[] args*/)
        {
            _health.TakeDamage(30 * Time.deltaTime);
            Debug.Log("Apply Damage");
        }
    }
}
