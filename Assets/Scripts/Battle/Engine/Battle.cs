using System;
using System.Collections.Generic;
using System.Linq;
using BattleEvents;

public class Battle
{
    public Witch Witch { get; private set; }
    public IList<Creature> Creatures { get; private set; }
    public ILogger Logger { get; private set; }
    public Random Rand { get; }
    public IList<bool> Cheats => cheats;
    public float DamagePositiveMod { get; private set; }
    public float DamageNegativeMod { get; private set; }
    public float HealPositiveMod { get; private set; }
    public float HealNegativeMod { get; private set; }

    private IList<Battler> turnOrder;
    private int turnCounter;
    private bool[] cheats;
    private MatrixInt typeChart;

    public int CompareElements(Element att, Element def)
    {
        return typeChart.At((int)att - 1, (int)def - 1);
    }

    public Battle(Witch witch, IList<Creature> creatures, ILogger logger, MatrixInt typeChart)
    {
        Witch = witch;
        Creatures = creatures;
        Logger = logger;
        this.typeChart = typeChart;

        Rand = new Random();

        Witch.Battle = this;
        foreach (Creature c in Creatures)
        {
            c.Battle = this;
        }

        turnOrder = new List<Battler>();

        turnOrder.Add(witch);
        foreach (Battler c in creatures)
            turnOrder.Add(c);

        turnCounter = 0;

        cheats = new bool[Enum.GetNames(typeof(Cheats)).Length];
    }

    public void SetModifiers(float damagePositive, float damageNegative,
        float healPositive, float healNegative)
    {
        DamagePositiveMod = damagePositive;
        DamageNegativeMod = damageNegative;
        HealPositiveMod = healPositive;
        HealNegativeMod = healNegative;
    }

    public IEnumerable<BattleEvent> Run()
    {
        while (true)
        {
            IEnumerable<BattleEvent> turnIter = turnOrder[turnCounter].Act();
            foreach (BattleEvent ev in turnIter)
            {
                yield return ev;

                RetireDeadCreatures();
            }

            yield return new EndTurnEvent(turnOrder[turnCounter]);

            if (IsOver())
            {
                yield break;
            }

            turnCounter = (turnCounter + 1) % turnOrder.Count;
        }
    }

    public bool IsOver()
    {
        if (Creatures.Select(c => c.Health).Sum() == 0)
            return true;

        if (Witch.Health == 0)
            return true;

        return false;
    }

    private void LogBattlers()
    {
        string witch = $"Witch [HP: {Witch.Health}]";
        string creatures = string.Join(" | ", Creatures
            .Select(cr => $"{cr.Name} [HP: {cr.Health}]"));
        string text = witch + " | " + creatures;
        Logger.Log($"Status: {text}");
    }

    private void RetireDeadCreatures()
    {
        turnOrder = new List<Battler>(turnOrder.Where(b => b.Health > 0));
    }
}
