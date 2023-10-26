using System;
using System.Collections.Generic;
using System.Linq;

public class Battle
{
    public Witch Witch { get; private set; }
    public IList<Creature> Creatures { get; private set; }
    public ILogger Logger { get; private set; }
    public Random Rand { get; }

    private IList<Battler> turnOrder;
    private int turnCounter;

    public Battle(Witch witch, IList<Creature> creatures, ILogger logger)
    {
        Witch = witch;
        Creatures = creatures;
        Logger = logger;

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
    }

    public void Run()
    {
        LogBattlers();

        while (!IsOver())
        {
            turnOrder[turnCounter].Act();
            turnCounter = (turnCounter + 1) % turnOrder.Count;
            LogBattlers();
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
}
