// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 25.10.23
// Author: melonanas1@gmail.com
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using Stats;
using UnityEngine;

namespace Items
{
    public class StatPill : Item
    {
        [SerializeField] Stat _stat;
        CharacterStats _activeStats;

        [SerializeField] float changeValue = 1.2f;
        [SerializeField] float duration = 5f;

        public override void Use(GameObject user)
        {
            _activeStats = user.GetComponent<CharacterStats>();
            changeValue *= _activeStats.GetStat(Stat.PillEfficiency);
            StartCoroutine(UpdatingStats());
        }

        public IEnumerator UpdatingStats()
        {
            _activeStats.MultipliStatValue(_stat, changeValue);
            yield return new WaitForSeconds(duration);
            _activeStats.MultipliStatValue(_stat, 1 / changeValue);
            Destroy(gameObject);
        }

        public override bool CollectConditionIsFullfilled(GameObject user)
        {
            return true;
        }
    }
}