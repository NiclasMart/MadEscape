using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "LootTabel", menuName = "Scriptable Objects/LootTabel")]
    public class LootTable : ScriptableObject
    {
        [Range(0, 1)] public float PrimaryLootChance;
        public List<LootEntry> PrimaryLoot;
        [Range(0, 1)] public float SecondaryLootChance;
        public List<LootEntry> SecondaryLoot;
    }

    [System.Serializable]
    public class LootEntry
    {
        public Item Item;
        public int Weight;
    }
}
