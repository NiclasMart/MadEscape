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
            

            public Healer(SkillTemplate.SkillInfo info, GameObject user) : base(info, user)
            {
                _health = user.GetComponent<Health>();
            }

            public override void InitializeAfterTemplateCreation() { }

            protected override void StartSkillEffect() { }

            protected override void SkillEffect()
            {
                _health.RegenerateHealth(Amount * Time.deltaTime);
            }

            protected override void EndSkillEffect() { }
        }

        public class DamageArea : Skill
        {
            public float Size;
            public GameObject AreaVisualsPrefab;

            private GameObject _areaRef;


            public DamageArea(SkillTemplate.SkillInfo info, GameObject target) : base(info, target) { }

            public override void InitializeAfterTemplateCreation()
            {
                _areaRef = Object.Instantiate(AreaVisualsPrefab, User.transform.position, Quaternion.identity, User.transform);
                _areaRef.SetActive(false);
            }

            protected override void StartSkillEffect()
            {
                _areaRef.SetActive(true);
            }

            protected override void SkillEffect()
            {
                
            }

            protected override void EndSkillEffect()
            {
                _areaRef.SetActive(false);
            }
        }
    }
}
