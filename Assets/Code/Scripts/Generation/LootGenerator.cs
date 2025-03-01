using Items;
using UnityEngine;

namespace Generation
{
    public class LootGenerator
    {
        LootTable _lootTable;
        int _primaryLootWeight;
        int _secondaryLootWeight;

        public LootGenerator(LootTable lootTable)
        {
            _lootTable = lootTable;
            foreach (var entry in lootTable.PrimaryLoot)
            {
                _primaryLootWeight += entry.Weight;
            }

            foreach (var entry in lootTable.SecondaryLoot)
            {
                _secondaryLootWeight += entry.Weight;
            }
        }

        public Item Generate()
        {
            // generate primary loot
            float rand = Random.Range(0f, 1f);
            if (rand <= _lootTable.PrimaryLootChance)
            {
                int weight = Random.Range(0, _primaryLootWeight + 1);
                int calculatedWeight = 0;
                foreach (var entry in _lootTable.PrimaryLoot)
                {
                    calculatedWeight += entry.Weight;
                    if (calculatedWeight >= weight)
                    {
                        return entry.Item;
                    }
                }
            }

            // generate secondary loot
            rand = Random.Range(0f, 1f);
            if (rand <= _lootTable.SecondaryLootChance)
            {
                int weight = Random.Range(0, _secondaryLootWeight + 1);
                int calculatedWeight = 0;
                foreach (var entry in _lootTable.SecondaryLoot)
                {
                    calculatedWeight += entry.Weight;
                    if (calculatedWeight >= weight)
                    {
                        return entry.Item;
                    }
                }
            }

            return null;
        }
    }
}
