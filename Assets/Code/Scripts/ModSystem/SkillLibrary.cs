using Controller;
using Core;
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

            public Healer() { }

            public Healer(SkillTemplate.SkillInfo info, GameObject user) : base(info, user)
            {
                _health = user.GetComponent<Health>();
            }

            protected override void InitializeAfterTemplateCreation() { }

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
            public float DamageRate;
            public float Damage;
            public GameObject AreaVisualsPrefab;

            private int _targetLayerMask;
            private GameObject _areaRef;
            private float _lastDamageTime = Mathf.NegativeInfinity;

            public DamageArea() { }

            public DamageArea(SkillTemplate.SkillInfo info, GameObject target) : base(info, target) { }

            protected override void InitializeAfterTemplateCreation()
            {
                _areaRef = Object.Instantiate(AreaVisualsPrefab, User.transform.position, Quaternion.identity, User.transform);
                _areaRef.transform.localScale = new Vector3(Size, _areaRef.transform.localScale.y, Size);
                _areaRef.SetActive(false);
                _targetLayerMask = ServiceProvider.Get<GameManager>().GetPlayer().gameObject.layer;
            }

            protected override void StartSkillEffect()
            {
                _areaRef.SetActive(true);
            }

            protected override void SkillEffect()
            {
                if (_lastDamageTime + DamageRate < Time.time)
                {
                    var targetHits = Physics.OverlapSphere(User.transform.position, Size, 1 << _targetLayerMask);

                    foreach (var targets in targetHits)
                    {
                        var player = targets.GetComponent<PlayerController>();
                        if (player != null)
                        {
                            player.Health.ApplyDamage(Damage);
                            Debug.Log("Apply damage to player");
                        }
                    }

                    _lastDamageTime = Time.time;
                }
            }
            
            protected override void EndSkillEffect()
            {
                _areaRef.SetActive(false);
            }
            
            public override void Reset()
            {
                base.Reset();
                _lastDamageTime = Mathf.NegativeInfinity;
            }
        }
    }
}
