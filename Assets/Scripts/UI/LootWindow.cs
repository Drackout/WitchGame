using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LootWindow : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private LootEntry lootEntryPrefab;

    public IList<(Item, int)> Loot
    {
        set
        {
            var lootGroups = value.GroupBy(l => l.Item1)
                .Select(group => (group.Key, group.Sum(l => l.Item2)));
            foreach ((Item item, int amount) in lootGroups)
            {
                LootEntry entry = Instantiate(lootEntryPrefab, container);
                entry.LootObject = item.GetLootObject();
                entry.Amount = amount;
            }
        }
    }
}
