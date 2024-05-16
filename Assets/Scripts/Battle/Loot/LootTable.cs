using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName="LootTable", menuName="ScriptableObjects/Loot Table")]
public class LootTable : ScriptableObject
{
    [SerializeField] private CreatureLootConfig[] entries;
    [SerializeField] private Item defaultItem;

    public (Item item, int amount) RollLoot(EnemyCreature creature)
    {
        CreatureLootConfig config = Array.Find(entries,
            (CreatureLootConfig c) => c.creature == creature);

        if (config == null)
        {
            return (defaultItem, 0);
        }

        var cumulativeProbabilities = new List<(ItemRateEntry, float)>();
        float sum = 0;
        foreach (ItemRateEntry e in config.itemRates)
        {
            sum += e.rate;
            cumulativeProbabilities.Add((e, sum));
        }

        float roll = Random.value * sum;
        foreach ((ItemRateEntry e, float prob) in cumulativeProbabilities)
        {
            if (roll < prob)
            {
                return (e.item, e.amount);
            }
        }

        return (defaultItem, 0);
    }

    private int CompareByRate(ItemRateEntry a, ItemRateEntry b)
    {
        return a.rate < b.rate ? -1 : a.rate > b.rate ? 1 : 0;
    }
}
