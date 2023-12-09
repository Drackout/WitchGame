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

    private IList<Battler> turnOrder;
    private int turnCounter;
    private bool[] cheats;

    public static int CompareElements(Element att, Element def)
    {
        if (att == Element.Fire)
        {
            if (def == Element.Water)
                return -1;
            else if (def == Element.Grass)
                return 1;
            return 0;
        }
        else if (att == Element.Water)
        {
            if (def == Element.Grass)
                return -1;
            else if (def == Element.Fire)
                return 1;
            return 0;
        }
        else if (att == Element.Grass)
        {
            if (def == Element.Fire)
                return -1;
            else if (def == Element.Water)
                return 1;
            return 0;
        }
        return 0;
    }

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

        cheats = new bool[Enum.GetNames(typeof(Cheats)).Length];
    }

    public IEnumerable<BattleEvent> Run()
    {
        while (!IsOver())
        {
            IEnumerable<BattleEvent> turnIter = turnOrder[turnCounter].Act();
            foreach (BattleEvent ev in turnIter)
            {
                yield return ev;
                RetireDeadCreatures();
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
