// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 25.10.23
// Author: melonanas1@gmail.com
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Items
{
    public class StatPill : Item
    {
        [SerializeField] Stat stat;
        CharacterStats activeStats;

        [SerializeField] float changeValue = 1.2f;
        [SerializeField] float duration = 5f;

        public override void Use(GameObject user)
        {
            activeStats = user.GetComponent<CharacterStats>();
            changeValue *= activeStats.GetStat(Stat.PillEfficiency);
            StartCoroutine(UpdatingStats());
        }

        public IEnumerator UpdatingStats()
        {
            activeStats.UpdateStat(stat, changeValue);
            yield return new WaitForSeconds(duration);
            activeStats.UpdateStat(stat, 1 / changeValue);
            Destroy(gameObject);
        }

        public override bool CollectConditionIsFullfilled(GameObject user)
        {
            return true;
        }
    }
}