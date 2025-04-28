using UnityEngine;
using VitalForces;

public class CharacterSkillLibrary
{
    public static void TestSkill(GameObject target)
    {
        target.GetComponent<Health>().TakeDamage(30 * Time.deltaTime);
    }
}
