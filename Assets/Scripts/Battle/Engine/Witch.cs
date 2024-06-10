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
    public IList<int> HeldCards { get; private set; }
    public int MaxActions { get; private set; }
    public IList<Card> IncludeInNextHand { get; private set; }

    public IList<Card> Hand => hand;
    public IList<Card> DiscardPile => discardPile;
    public IList<Card> Deck => deck;

    private IList<Card> deck;
    private List<Card> hand;
    private IList<Card> discardPile;

    public Witch(string name, int maxHealth, IEnumerable<Card> cards,
        int maxSlots, int startingSlots, int maxActions) : base(name, maxHealth)
    {
        MaxSlots = maxSlots;
        Slots = startingSlots;
        MaxActions = maxActions;
        CardsPlayed = 0;
        Input = new InputResponse();
        Shield = new Shield();

        deck = new List<Card>(cards);

        hand = new List<Card>();
        for (int i = 0; i < MaxSlots; i++)
        {
            hand.Add(Card.None);
        }

        discardPile = new List<Card>();

        HeldCards = new List<int>();
        IncludeInNextHand = new List<Card>();
    }

    public override IEnumerable<BattleEvent> Act()
    {
        int actionsDone = 0;
        bool actionDone = false;

        // Refill hand
        foreach (BattleEvent ev in RefillHand())
        {
            yield return ev;
        }

        yield return new StartTurnEvent(this);

        while (GetHandCount() > 0 && actionsDone < MaxActions)
        {
            actionDone = false;

            while (!actionDone)
            {
                yield return new InputRequestEvent(InputRequestType.Play);

                if (Input.Intention == Intention.Play)
                {
                    int cardIndex = Input.Selection;

                    Card card = hand[cardIndex];

                    if (CardsPlayed >= 2)
                    {
                        battle.Logger.Log("You can only play up to 2 cards per turn!");
                    }
                    else
                    {
                        if (HeldCards.Contains(cardIndex))
                        {
                            HeldCards.Remove(cardIndex);
                            actionsDone -= 1;
                        }

                        foreach (BattleEvent ev in PlayCard(card, cardIndex))
                        {
                            if (ev is PlayCardEvent)
                            {
                                CardsPlayed += 1;
                                actionDone = true;
                            }
                            yield return ev;
                        }

                        if (!actionDone)
                        {
                            continue;
                        }

                        foreach (BattleEvent ev in Discard(cardIndex))
                        {
                            yield return ev;
                        }

                        actionsDone += 1;
                    }
                }
                else if (Input.Intention == Intention.Hold)
                {
                    int cardIdx = Input.Selection;

                    if (HeldCards.Count >= 1)
                    {
                        battle.Logger.Log("You can only hold a single card!");

                        yield return new HoldEvent(cardIdx, false);
                    }
                    else
                    {
                        HeldCards.Add(cardIdx);
                        battle.Logger.Log($"{hand[cardIdx]} was held...");

                        yield return new HoldEvent(cardIdx, true);

                        actionDone = true;
                        actionsDone += 1;
                    }
                }
                else if (Input.Intention == Intention.Unhold)
                {
                    HeldCards.Remove(Input.Selection);

                    actionDone = true;
                    actionsDone -= 1;
                }
                else if (Input.Intention == Intention.EndTurn)
                {
                    if (CardsPlayed <= 0)
                    {
                        battle.Logger.Log("You must play at least 1 card!");
                    }
                    else
                    {
                        actionDone = true;
                        actionsDone = MaxActions;
                    }
                }
            }
        }

        // Throw out remaining cards, except held ones
        for (int i = 0; i < Slots; i++)
        {
            if (hand[i].Type != CardType.None)
            {
                Card c = hand[i];
                if (HeldCards.Contains(i))
                {
                    IncludeInNextHand.Add(c);
                    hand[i] = Card.None;
                    yield return new DiscardEvent(c, i);
                }
                else
                {
                    foreach (BattleEvent ev in Discard(i))
                    {
                        yield return ev;
                    }
                }
            }
        }

        HeldCards.Clear();

        // Move cards to the first slots, in order
        List<Card> newHand = hand.Where((Card c) => c.Type != CardType.None).ToList();
        while (newHand.Count < MaxSlots)
        {
            newHand.Add(Card.None);
        }
        hand = newHand;

        // Update slots
        int newSlots = CardsPlayed > 1 ? Slots - 1 : Slots + 1;
        newSlots = newSlots > MaxSlots ? MaxSlots : newSlots;

        yield return new SlotsEvent(Slots, newSlots);

        Slots = newSlots;

        CardsPlayed = 0;
    }

    public override BattleEvent Hurt(Attack attack)
    {
        if (Shield.Charges > 0)
        {
            int advantage = battle.CompareElements(attack.Element, Shield.Element);
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
        if (battle.Cheats[(int)Cheats.InfiniteHealth])
        {
            damage = 0;
        }
        Health = Math.Max(0, Health - damage);
        battle.Logger.Log($"You took {damage} damage!");
        return new DamageEvent(this, damage, attack.Element, 0, "none"); // 0: Cause witch doesnt React on dmg taken
    }

    private IEnumerable<BattleEvent> PlayCard(Card card, int index)
    {
        if (card.Type == CardType.Sword)
        {
            yield return new InputRequestEvent(InputRequestType.Target);
            if (Input.Intention == Intention.Cancel)
            {
                yield break;
            }
            yield return new PlayCardEvent(card, index);
                //battle.Logger.Log("CARD::: " + card.ToString());
            int targetIdx = Input.Selection;
            Battler target = battle.Creatures[targetIdx];

            battle.Logger.Log($"You used [{card}] on {target.Name}!");

            Attack attack = new Attack(card.Power, card.Element, new string[] { "melee" });
            yield return target.Hurt(attack);
        }
        else if (card.Type == CardType.Spell)
        {
            yield return new InputRequestEvent(InputRequestType.Target);
            if (Input.Intention == Intention.Cancel)
            {
                yield break;
            }
            yield return new PlayCardEvent(card, index);
                //battle.Logger.Log("CARD::: " + card.ToString());
            int targetIdx = Input.Selection;
            Battler target = battle.Creatures[targetIdx];

            battle.Logger.Log($"You used [{card}] on {target.Name}!");

            Attack attack = new Attack(card.Power, card.Element, new string[] { "ranged" });
            yield return target.Hurt(attack);
        }
        else if (card.Type == CardType.Shield)
        {
            yield return new PlayCardEvent(card, index);
            Shield = new Shield(card.Power, card.Element);
            battle.Logger.Log($"You used [{card}]! Got {Shield.Charges} charges of {Shield.Element} shield");
            yield return new ShieldEvent(Shield, Shield.Element);
        }
        else if (card.Type == CardType.Heal)
        {
            yield return new PlayCardEvent(card, index);
            int restored = GetEffectiveHeal(card).Item1;
            int reactionType = GetEffectiveHeal(card).Item2;
            battle.Logger.Log($"Healing for {restored}");
            Health = Math.Min(MaxHealth, Health + restored);
            yield return new HealEvent(restored, card.Element, reactionType);
        }
    }

    private IEnumerable<BattleEvent> RefillHand()
    {
        // Check each slot - if it's empty, draw a card and put it there
        for (int i = 0; i < Slots; i++)
        {
            RefillDeck();

            if (IncludeInNextHand.Count > 0)
            {
                hand[i] = IncludeInNextHand[i];
                IncludeInNextHand.RemoveAt(0);
                yield return new DrawEvent(hand[i]);
            }
            else if (deck.Count > 0 && hand[i].Type == CardType.None)
            {
                Card c = deck[0];
                hand[i] = c;
                deck.RemoveAt(0);
                yield return new DrawEvent(c);
                //battle.Logger.Log("DRAW!!");
            }
        }
        // battle.Logger.Log("Finish refill hand!!");

        // for (int i = 0; i < hand.Count; i++)
        // {
        //     battle.Logger.Log("CARD on witch: " + i);
        //     // Animator anim = cardContainer.transform.GetChild(i).GetComponent<Animator>();
        //     // anim.SetTrigger("pNormal");
        // }
    }

    private Tuple<int, int> GetEffectiveHeal(Card card)
    {
        int reactionType = 0; //-1-Weak, 0-Normal, 1-Strong

        if (Shield.Charges <= 0)
        {
            return Tuple.Create(card.Power, reactionType);
        }

        int advantage = battle.CompareElements(card.Element, Shield.Element);
        int restored = card.Power;

        if (advantage > 0)
        {
            restored = (int)MathF.Floor(restored * battle.HealPositiveMod);
            reactionType = 1;
        }
        else if (advantage < 0)
        {
            restored = (int)MathF.Floor(restored * battle.HealNegativeMod);
            reactionType = -1;
        }

        return Tuple.Create(restored, reactionType);
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

    private IEnumerable<BattleEvent> Discard(int index)
    {
        Card c = hand[index];
        hand[index] = Card.None;
        discardPile.Add(c);
        yield return new DiscardEvent(c, index);
    }

    private int GetHandCount()
    {
        return hand.Where((Card c) => c.Type != CardType.None).Count();
    }

    private void LogHand()
    {
        string text = string.Join(" | ", hand.Select(c => c.ToString()));
        battle.Logger.Log("Hand: " + text);
    }

    protected override void Setup()
    {
        bool shouldShuffle = BattleSettings.Instance?.ShuffleDeck ?? true;
        if (shouldShuffle)
        {
            battle.Rand.Shuffle(deck);
            battle.Logger.Log("Deck shuffled");
        }
    }

    public int ShowOneCard(int cardIndex)
    {
        return hand[cardIndex].Power;
    }
}
