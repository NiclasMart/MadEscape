// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 31.05.25
// Author: langner.paul@t-online.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stats;
using VitalForces;

namespace Helper
{
    public class TargetDummy : MonoBehaviour
    {
        private CharacterStats _stats;
        private Health _health;
        private List<StatRecord> _enemyStatsList;
        private Dictionary<Stat, float> _dummyStats;
        [SerializeField] private bool _useEnemyData = true;
        [SerializeField] private int _statID = 0;
        [SerializeField] private float _lifeStat = 100f;
        [SerializeField] private float _lifeRegenStat = 0f;
        [SerializeField] private float _armorStat = 0f;

        void Awake()
        {
            _stats = GetComponent<CharacterStats>();
            _health = GetComponent<Health>();
        }

        private void Start()
        {
            if (_useEnemyData)
            {
                // Use enemy data if specified
                _enemyStatsList = LoadStats.LoadEnemyStats();
                _dummyStats = _enemyStatsList[_statID].statDict;
            }
            else
            {
                // Use alternative dummy stats if not using enemy data
                _dummyStats = GetAlternativeDummyStats();
            }
            Initialize(_dummyStats);
        }

        void Initialize(Dictionary<Stat, float> baseStats)
        {
            _stats.Initialize(baseStats);
            _health.Initialize(_stats);
        }

        private void OnEnable()
        {
            // Subscribe to callbacks when the object is enabled
            if (_health != null)
            {
                _health.OnDeath += HandleDeath;
            }
        }

        private void OnDisable()
        {
            // Unsubscribe from callbacks when the object is disabled
            if (_health != null)
            {
                _health.OnDeath -= HandleDeath;
            }
        }

        private void HandleDeath(GameObject self)
        {
            StartCoroutine(ReviveAfterDelay(0.5f));
        }

        private IEnumerator ReviveAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            Initialize(_dummyStats);
        }

        private Dictionary<Stat, float> GetAlternativeDummyStats()
        {
            return new Dictionary<Stat, float>
                {
                    { Stat.Life, _lifeStat },
                    { Stat.Armor, _armorStat },
                    { Stat.LifeRegen, _lifeRegenStat }
                };
        }
    }
}