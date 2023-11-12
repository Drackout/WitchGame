using System;
using System.Collections.Generic;
using System.Linq;
using BattleEvents;

public class Witch : Battler
{
    public int MaxSlots { get; private set; }
    public int Slots { get; private set; }
    public Shield Shield { get; private set; }
    public int CardsPlayed { get; private set; }
    public InputResponse Input { get; set; }
    public bool InfiniteHealth { get; set; }
    public IList<Card> HeldCards { get; private set; }

    public IList<Card> Hand => hand;
    public IList<Card> DiscardPile => discardPile;
    public IList<Card> Deck => deck;

    private IList<Card> deck;
    private IList<Card> hand;
    private IList<Card> discardPile;

    public Witch(string name, int maxHealth, IEnumerable<Card> cards,
        int maxSlots, int startingSlots) : base(name, maxHealth)
    {
        MaxSlots = maxSlots;
        Slots = startingSlots;
        CardsPlayed = 0;
        Input = new InputResponse();
        Shield = new Shield();

        deck = new List<Card>(cards);
        hand = new List<Card>();
        discardPile = new List<Card>();
        HeldCards = new List<Card>();
    }

    public override IEnumerable<BattleEvent> Act()
    {
        int actionsDone = 0;

        // Refill hand
        foreach (BattleEvent ev in RefillHand())
        {
            yield return ev;
        }

        while (actionsDone < 3)
        {
            yield return new InputRequestEvent(InputRequestType.Play);

            if (Input.Intention == Intention.Play)
            {
                Card card = hand[Input.Selection];

                if (CardsPlayed >= 2)
                {
                    battle.Logger.Log("You can only play up to 2 cards per turn!");
                }
                else if (HeldCards.Contains(card))
                {
                    battle.Logger.Log($"{card} is being held!");
                }
                else
                {
                    foreach (BattleEvent ev in PlayCard(card))
                    {
                        yield return ev;
                    }

                    if (battle.IsOver())
                    {
                        break;
                    }

                    actionsDone += 1;
                }
            }
            else if (Input.Intention == Intention.Hold)
            {
                if (HeldCards.Count >= 1)
                {
                    battle.Logger.Log("You can only hold a single card!");
                }
                else
                {
                    int cardIdx = Input.Selection;
                    HeldCards.Add(hand[cardIdx]);
                    battle.Logger.Log($"{hand[cardIdx]} was held...");

                    actionsDone += 1;
                }
            }
            else if (Input.Intention == Intention.EndTurn)
            {
                if (CardsPlayed <= 0)
                {
                    battle.Logger.Log("You must play at least 1 card!");
                }
                else
                {
                    break;
                }
            }
        }

        // Update slots
        int newSlots = CardsPlayed > 1 ? Slots - 1 : Slots + 1;
        newSlots = newSlots > MaxSlots ? MaxSlots : newSlots;

        yield return new SlotsEvent(Slots, newSlots);

        Slots = newSlots;

        // Throw out remaining cards
        int ptr = 0;
        while (ptr < hand.Count)
        {
            Card c = hand[ptr];
            if (!HeldCards.Contains(c))
            {
                Discard(c);
                yield return new DiscardEvent(c, 0);
            }
            else
            {
                ptr++;
            }
        }

        HeldCards.Clear();
        CardsPlayed = 0;
    }

    public override BattleEvent Hurt(Attack attack)
    {
        if (Shield.Charges > 0)
        {
            int advantage = Battle.CompareElements(attack.Element, Shield.Element);
            battle.Logger.Log($"Checking shield - {attack.Element} vs {Shield.Element}");
            if (advantage < 0)
            {
                int charges = Shield.Charges - 1;
                Shield = new Shield(charges, Shield.Element);
                battle.Logger.Log($"Shield block! {Shield.Charges} charges remaining");
                return new BlockEvent();
            }
            else
            {
                battle.Logger.Log($"No block");
            }
        }

        int damage = attack.Power;
        if (InfiniteHealth)
        {
            damage = 0;
        }
        Health = Math.Max(0, Health - damage);
        battle.Logger.Log($"You took {damage} damage!");
        return new DamageEvent(this, damage);
    }

    private IEnumerable<BattleEvent> PlayCard(Card card)
    {
        Discard(card);
        CardsPlayed += 1;

        if (card.Type == CardType.Sword)
        {
            yield return new InputRequestEvent(InputRequestType.Target);
            yield return new PlayCardEvent(card);
            int targetIdx = Input.Selection;
            Battler target = battle.Creatures[targetIdx];

            battle.Logger.Log($"You used [{card}] on {target.Name}!");

            Attack attack = new Attack(card.Power, card.Element, new string[] { "melee" });
            yield return target.Hurt(attack);
        }
        else if (card.Type == CardType.Spell)
        {
            yield return new InputRequestEvent(InputRequestType.Target);
            yield return new PlayCardEvent(card);
            int targetIdx = Input.Selection;
            Battler target = battle.Creatures[targetIdx];

            battle.Logger.Log($"You used [{card}] on {target.Name}!");

            Attack attack = new Attack(card.Power, card.Element, new string[] { "ranged" });
            yield return target.Hurt(attack);
        }
        else if (card.Type == CardType.Shield)
        {
            yield return new PlayCardEvent(card);
            Shield = new Shield(card.Power, card.Element);
            battle.Logger.Log($"You used [{card}]! Got {Shield.Charges} charges of {Shield.Element} shield");
            yield return new ShieldEvent(Shield);
        }
        else if (card.Type == CardType.Heal)
        {
            yield return new PlayCardEvent(card);
            int restored = GetEffectiveHeal(card);
            battle.Logger.Log($"Healing for {restored}");
            Health = Math.Min(MaxHealth, Health + restored);
            yield return new HealEvent(card.Power);
        }
    }

    private IEnumerable<BattleEvent> RefillHand()
    {
        // While hand is not full...
        while (hand.Count < Slots)
        {
            RefillDeck();

            if (deck.Count > 0)
            {
                Card c = deck[0];
                hand.Add(c);
                deck.RemoveAt(0);
                yield return new DrawEvent(c);
            }
            else
            {
                break;
            }
        }
    }

    private int GetEffectiveHeal(Card card)
    {
        if (Shield.Charges <= 0)
        {
            return card.Power;
        }

        int advantage = Battle.CompareElements(card.Element, Shield.Element);
        int restored = card.Power;
        if (advantage > 0)
        {
            restored *= 2;
        }
        else if (advantage < 0)
        {
            restored /= 2;
        }

        return restored;
    }

    private void RefillDeck()
    {
        // If deck is empty...
        if (deck.Count == 0)
        {
            // If discard is not empty, shuffle discard and make it
            // the new deck
            if (discardPile.Count > 0)
            {
                battle.Rand.Shuffle(discardPile);
                foreach (Card c in discardPile)
                    deck.Add(c);
                discardPile.Clear();
            }
        }
    }

    private void Discard(Card card)
    {
        hand.Remove(card);
        discardPile.Add(card);
    }

    private void LogHand()
    {
        string text = string.Join(" | ", hand.Select(c => c.ToString()));
        battle.Logger.Log("Hand: " + text);
    }

}
