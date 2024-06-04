using System;

[Serializable]
public class CreatureLootConfig
{
    public EnemyCreature creature;
    public ItemRateEntry[] itemRates;
    public int gold;
}
