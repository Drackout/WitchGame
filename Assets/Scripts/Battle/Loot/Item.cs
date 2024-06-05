using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] protected GameObject lootPrefab;

    public virtual GameObject GetLootObject()
    {
        return Instantiate(lootPrefab);
    }
}
