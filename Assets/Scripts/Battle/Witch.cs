using System;
using System.Collections.Generic;
using System.Linq;
using BattleEvents;

public class Witch : Battler
{
    public int MaxSlots { get; private set; }
    public int Slots { get; private set; }
    public int CardsPlayed { get; private set; }
    public int Input { get; set; }
    public bool InfiniteHealth { get; set; }

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
        Input = 0;

        deck = new List<Card>(cards);
        hand = new List<Card>();
        discardPile = new List<Card>();
    }

    public override IEnumerable<BattleEvent> Act()
    {
        foreach (BattleEvent ev in RefillHand())
        {
            yield return ev;
        }

        yield return new InputRequestEvent(InputRequestType.Play);

        battle.Logger.Log($"{Input}");
        int cardIdx = Input;
        if (cardIdx >= 0)
        {
            Card card = hand[cardIdx];
            foreach (BattleEvent ev in PlayCard(card))
            {
                yield return ev;
            }
        }

        if (battle.IsOver())
            yield break;

        if (hand.Count > 0)
        {
            LogHand();
            yield return new InputRequestEvent(InputRequestType.Play);
            cardIdx = Input;
            if (cardIdx >= 0)
            {
                Card card = hand[cardIdx];
                foreach (BattleEvent ev in PlayCard(card))
                {
                    yield return ev;
                }
            }
        }

        Slots = CardsPlayed > 1 ? Slots - 1 : Slots + 1;
        Slots = Slots > MaxSlots ? MaxSlots : Slots;

        // Throw out remaining cards
        while (hand.Count > 0)
        {
            Card c = hand[0];
            Discard(hand[0]);
            yield return new DiscardEvent(c, 0);
        }

        CardsPlayed = 0;
    }

    public override BattleEvent Hurt(Attack attack)
    {
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
            int targetIdx = Input;
            Battler target = battle.Creatures[targetIdx];

            battle.Logger.Log($"You used [{card}] on {target.Name}!");

            Attack attack = new Attack(card.Power, card.Element, new string[] { "melee" });
            yield return target.Hurt(attack);
        }
        else if (card.Type == CardType.Spell)
        {
            yield return new InputRequestEvent(InputRequestType.Target);
            yield return new PlayCardEvent(card);
            int targetIdx = Input;
            Battler target = battle.Creatures[targetIdx];

            battle.Logger.Log($"You used [{card}] on {target.Name}!");

            Attack attack = new Attack(card.Power, card.Element, new string[] { "ranged" });
            yield return target.Hurt(attack);
        }
        else if (card.Type == CardType.Shield)
        {
            yield return new PlayCardEvent(card);
            battle.Logger.Log($"You used [{card}]! Nothing happens... (yet)");
        }
        else if (card.Type == CardType.Heal)
        {
            yield return new PlayCardEvent(card);
            battle.Logger.Log($"You used [{card}]! Nothing happens... (yet)");
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
