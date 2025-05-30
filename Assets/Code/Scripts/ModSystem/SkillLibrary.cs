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
            private Collider[] _hitBuffer;
            private float _lastDamageTime = Mathf.NegativeInfinity;

            public DamageArea() { }

            public DamageArea(SkillTemplate.SkillInfo info, GameObject target) : base(info, target)
            {
                GameManager gameManager = ServiceProvider.Get<GameManager>();
                _hitBuffer = new Collider[gameManager.PlayerCount];
                _targetLayerMask = gameManager.GetPlayer().gameObject.layer;
            }

            protected override void InitializeAfterTemplateCreation()
            {
                _areaRef = Object.Instantiate(AreaVisualsPrefab, User.transform.position, Quaternion.identity, User.transform);
                _areaRef.transform.localScale = new Vector3(Size, _areaRef.transform.localScale.y, Size);
                _areaRef.SetActive(false);
            }

            protected override void StartSkillEffect()
            {
                _areaRef.SetActive(true);
            }

            protected override void SkillEffect()
            {
                if (!(_lastDamageTime + DamageRate < Time.time)) return;
                
                int hitCount = Physics.OverlapSphereNonAlloc(User.transform.position, Size, _hitBuffer, 1 << _targetLayerMask);

                for (int i = 0; i < hitCount; i++)
                {
                    var player = _hitBuffer[i].GetComponent<PlayerController>();
                    if (player == null) continue;
                    
                    player.Health.ApplyDamage(Damage);
                }
                _lastDamageTime = Time.time;
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
