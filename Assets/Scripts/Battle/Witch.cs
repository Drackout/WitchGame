using System;
using System.Collections.Generic;
using System.Linq;

public class Witch : Battler
{
    public int MaxSlots { get; private set; }
    public int Slots { get; private set; }
    public int CardsPlayed { get; private set; }

    public IEnumerable<Card> Hand => hand;
    public IEnumerable<Card> Discard => discard;
    public IEnumerable<Card> Deck => deck;

    private IPlayer player;
    private IList<Card> deck;
    private IList<Card> hand;
    private IList<Card> discard;

    public Witch(string name, int maxHealth, IEnumerable<Card> cards,
        int maxSlots, int startingSlots, IPlayer player) : base(name, maxHealth)
    {
        MaxSlots = maxSlots;
        Slots = startingSlots;
        CardsPlayed = 0;
        this.player = player;

        deck = new List<Card>(cards);
        hand = new List<Card>();
        discard = new List<Card>();
    }

    public override void Act()
    {
        RefillHand();

        LogHand();

        int cardIdx = player.DecideCard(battle);
        if (cardIdx >= 0)
        {
            Card card = hand[cardIdx];
            PlayCard(card);
        }

        if (hand.Count > 0)
        {
            cardIdx = player.DecideCard(battle);
            if (cardIdx >= 0)
            {
                Card card = hand[cardIdx];
                PlayCard(card);
            }
        }

        Slots = CardsPlayed > 1 ? Slots - 1 : Slots + 1;
        Slots = Slots > MaxSlots ? MaxSlots : Slots;

        CardsPlayed = 0;
    }

    public override void Hurt(Attack attack)
    {
        int damage = attack.Power;
        Health = Math.Max(0, Health - damage);
        battle.Logger.Log($"You took {damage} damage!");
    }

    private void PlayCard(Card card)
    {
        if (card.Type == CardType.Sword)
        {
            int targetIdx = player.DecideTarget(battle);
            Battler target = battle.Creatures[targetIdx];

            battle.Logger.Log($"You used [{card}] on {target.Name}!");

            Attack attack = new Attack(card.Power, card.Element,new string[] { "melee" });
            target.Hurt(attack);
        }
        else if (card.Type == CardType.Spell)
        {
            int targetIdx = player.DecideTarget(battle);
            Battler target = battle.Creatures[targetIdx];

            battle.Logger.Log($"You used [{card}] on {target.Name}!");

            Attack attack = new Attack(card.Power, card.Element, new string[] { "ranged" });
            target.Hurt(attack);
        }
        else if (card.Type == CardType.Shield)
        {
            battle.Logger.Log($"You used [{card}]! Nothing happens... (yet)");
        }
        else if (card.Type == CardType.Heal)
        {
            battle.Logger.Log($"You used [{card}]! Nothing happens... (yet)");
        }

        hand.Remove(card);
        discard.Add(card);
        CardsPlayed += 1;
    }

    private void RefillHand()
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
            deck.RemoveAt(0);
        }
    }

    private void LogHand()
    {
        string text = string.Join(" | ", hand.Select(c => c.ToString()));
        battle.Logger.Log("Hand: " + text);
    }
}
