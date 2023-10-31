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

    public IList<Card> Hand => hand;
    public IEnumerable<Card> Discard => discard;
    public IEnumerable<Card> Deck => deck;

    private IList<Card> deck;
    private IList<Card> hand;
    private IList<Card> discard;

    public Witch(string name, int maxHealth, IEnumerable<Card> cards,
        int maxSlots, int startingSlots) : base(name, maxHealth)
    {
        MaxSlots = maxSlots;
        Slots = startingSlots;
        CardsPlayed = 0;
        Input = 0;

        deck = new List<Card>(cards);
        hand = new List<Card>();
        discard = new List<Card>();
    }

    public override IEnumerable<BattleEvent> Act()
    {
        foreach (BattleEvent ev in RefillHand())
        {
            yield return ev;
        }

        LogHand();

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

        CardsPlayed = 0;
    }

    public override BattleEvent Hurt(Attack attack)
    {
        int damage = attack.Power;
        Health = Math.Max(0, Health - damage);
        battle.Logger.Log($"You took {damage} damage!");
        return new DamageEvent(this, damage);
    }

    private IEnumerable<BattleEvent> PlayCard(Card card)
    {
        hand.Remove(card);
        discard.Add(card);
        CardsPlayed += 1;

        if (card.Type == CardType.Sword)
        {
            yield return new InputRequestEvent(InputRequestType.Target);
            yield return new CardEvent(card);
            int targetIdx = Input;
            Battler target = battle.Creatures[targetIdx];

            battle.Logger.Log($"You used [{card}] on {target.Name}!");

            Attack attack = new Attack(card.Power, card.Element, new string[] { "melee" });
            yield return target.Hurt(attack);
        }
        else if (card.Type == CardType.Spell)
        {
            yield return new InputRequestEvent(InputRequestType.Target);
            yield return new CardEvent(card);
            int targetIdx = Input;
            Battler target = battle.Creatures[targetIdx];

            battle.Logger.Log($"You used [{card}] on {target.Name}!");

            Attack attack = new Attack(card.Power, card.Element, new string[] { "ranged" });
            yield return target.Hurt(attack);
        }
        else if (card.Type == CardType.Shield)
        {
            yield return new CardEvent(card);
            battle.Logger.Log($"You used [{card}]! Nothing happens... (yet)");
        }
        else if (card.Type == CardType.Heal)
        {
            yield return new CardEvent(card);
            battle.Logger.Log($"You used [{card}]! Nothing happens... (yet)");
        }
    }

    private IEnumerable<BattleEvent> RefillHand()
    {
        // While hand is not full...
        while (hand.Count < Slots)
        {
            // If deck is empty...
            if (deck.Count == 0)
            {
                // If discard is not empty, shuffle discard and make it
                // the new deck
                if (discard.Count > 0)
                {
                    battle.Rand.Shuffle(discard);
                    foreach (Card c in discard)
                        deck.Add(c);
                    discard.Clear();
                }
                // Deck and discard empty, no more cards
                else
                    break;
            }

            hand.Add(deck[0]);
            yield return new DrawEvent(deck[0]);
            deck.RemoveAt(0);
        }
    }

    private void LogHand()
    {
        string text = string.Join(" | ", hand.Select(c => c.ToString()));
        battle.Logger.Log("Hand: " + text);
    }
}
