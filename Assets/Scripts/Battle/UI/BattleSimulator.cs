using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using BattleEvents;
using TMPro;

public class BattleSimulator : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject cardContainer;
    [SerializeField] private ResourceBar playerHealthBar;
    [SerializeField] private UIShield playerShield;
    [SerializeField] private GameObject creatureContainer;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TMP_Text infiniteHealthText;
    [SerializeField] private TMP_Text drawPileTotalText;
    [SerializeField] private TMP_Text discardPileTotalText;
    [SerializeField] private TMP_Text NmbReceived;
    [SerializeField] private TMP_Text reactions;
    [SerializeField] private CardActionsDialog cardActionDialogPrefab;
    [SerializeField] private BattleLog battleLogger;
    [SerializeField] private UISlots slots;
    [SerializeField] private Image animationShield;

    private Battle battle;
    private IDictionary<Battler, UICreature> creatureElements;

    private InputResponse input;

    private CardActionsDialog activeCardActionDialog;

    private string logText;

    private int enemiesDefeated;

    private Animator Animator;

    private CardAnimation cardAnimation;

    private void Start()
    {
        creatureElements = new Dictionary<Battler, UICreature>();
        System.Random rnd = new System.Random();
        Animator = gameObject.GetComponentInChildren<Animator>();
        enemiesDefeated = 0;

        IList<Card> cards = new List<Card>
        {
            new Card(CardType.Shield, Element.Water, 2),
            new Card(CardType.Heal, Element.Fire, 2),
            new Card(CardType.Sword, Element.Water, 3),
            new Card(CardType.Heal, Element.Grass, 2),
            new Card(CardType.Sword, Element.Grass, 3),
            new Card(CardType.Shield, Element.Grass, 2),
            new Card(CardType.Shield, Element.Water, 2),
            new Card(CardType.Sword, Element.Water, 2),
            new Card(CardType.Heal, Element.Fire, 2),
            new Card(CardType.Shield, Element.Grass, 1),
            new Card(CardType.Sword, Element.Fire, 2),
            new Card(CardType.Spell, Element.Water, 3),
            new Card(CardType.Spell, Element.Grass, 3),
            new Card(CardType.Spell, Element.Water, 2),
            new Card(CardType.Spell, Element.Fire, 2),
            new Card(CardType.Spell, Element.Water, 3),
            new Card(CardType.Shield, Element.Fire, 2),
            new Card(CardType.Spell, Element.Fire, 3),
            new Card(CardType.Heal, Element.Water, 2),
            new Card(CardType.Sword, Element.Grass, 2),
            new Card(CardType.Spell, Element.Grass, 2),
            new Card(CardType.Heal, Element.Grass, 2),
            new Card(CardType.Shield, Element.Water, 2),
            new Card(CardType.Shield, Element.Fire, 2),
            new Card(CardType.Spell, Element.Grass, 3),
            new Card(CardType.Sword, Element.Fire, 3),
            new Card(CardType.Shield, Element.Grass, 2),
            new Card(CardType.Heal, Element.Grass, 2)
        };

        Witch witch = new Witch("Witch", 20, cards, 5, 4);

        IList<Creature> creatures = new List<Creature>
        {
            new Dummy("Dummy 1", 10, Element.Grass),
            new Dummy("Dummy 2", 15, Element.Fire),
            new Dummy("Dummy 3", 10, Element.Water)
        };

        ILogger logger = new UnityLogger();

        battle = new Battle(witch, creatures, battleLogger);

        for (int i = 0; i < battle.Creatures.Count; i++)
        {
            int iCopy = i;
            Creature c = battle.Creatures[i];
            Transform cc = creatureContainer.transform.GetChild(i);
            creatureElements[c] = cc.GetComponent<UICreature>();
            creatureElements[c].SetHealth(c.Health, c.MaxHealth);
            creatureElements[c].Element = c.Element;
            creatureElements[c].TargetButton.onClick.AddListener(
                () => HandleSelection(iCopy));
        }

        for (int i = 0; i < cardContainer.transform.childCount; i++)
        {
            int iCopy = i;
            Button b = cardContainer.transform.GetChild(i).GetComponent<Button>();
            b.onClick.AddListener(() => HandleCardClick(iCopy, b));
            b.interactable = false;

        }

        endTurnButton.onClick.AddListener(HandleEndTurnClick);
        endTurnButton.interactable = false;

        playerShield.Shield = new Shield();

        slots.Slots = battle.Witch.Slots;
        playerHealthBar.Set(battle.Witch.Health, battle.Witch.MaxHealth);

        StartCoroutine(RunBattle(battle));
    }

    private IEnumerator RunBattle(Battle battle)
    {
        IEnumerable<BattleEvent> battleIter = battle.Run();
        foreach (BattleEvent battleEvent in battleIter)
        {
            switch (battleEvent)
            {
                case InputRequestEvent ev:
                    if (ev.Type == InputRequestType.Play)
                    {
                        Debug.Log("[DEBUG] Choose a card");
                        ToggleCards(true);
                        endTurnButton.interactable = true;
                    }
                    else if (ev.Type == InputRequestType.Target)
                    {
                        Debug.Log("A CARD WAS SELECTED");
                        Debug.Log("[DEBUG] Choose a target");
                        ToggleTargets(true);
                    }
                    input = new InputResponse();
                    yield return new WaitUntil(() => input.Intention != Intention.None);
                    battle.Witch.Input = input;
                    if (ev.Type == InputRequestType.Play)
                    {
                        ToggleCards(false);
                        endTurnButton.interactable = false;
                    }
                    else if (ev.Type == InputRequestType.Target)
                    {
                        ToggleTargets(false);
                    }
                    break;
                case DrawEvent ev:
                    ShowHand(battle);
                    // Debug.Log("A");
                    yield return new WaitForSeconds(0.5f);
                    break;
                case MoveEvent ev:
                    // ENEMY TURN (1 by 1)
                    for (int i = 0; i < battle.Creatures.Count; i++)
                    {
                        if (battle.Creatures[i].Name == ev.Battler.Name)
                            creatureElements[battle.Creatures[i]].playAnimation("Attack", 99);
                    }
                    yield return new WaitForSeconds(0.5f);
                    break;
                case PlayCardEvent ev:
                    Debug.Log($"[DEBUG] Played {ev.Card}");
                    logText = $"Played {ev.Card}";
                    ShowHand(battle);
                    //playCardAnimation("pExit");
                    break;
                case DiscardEvent ev:
                    ShowHand(battle);
                    // Debug.Log("B");
                    yield return new WaitForSeconds(0.5f);
                    break;
                case SlotsEvent ev:
                    slots.Slots = ev.Current;
                    ShowHand(battle);

                    // Force all cards to starting position
                    for (int i = 0; i < cardContainer.transform.childCount; i++)
                    {
                        Animator anim = cardContainer.transform.GetChild(i).GetComponent<Animator>();
                        anim.SetTrigger("pNormal");
                    }

                    break;
                case DamageEvent ev:
                    if (ev.Target == battle.Witch)
                    {
                        // Debug.Log("Target: " + ev.Target);
                        playerHealthBar.Set(battle.Witch.Health, battle.Witch.MaxHealth);
                        setNumbersReceived(ev.Damage, ev.Element, "Damage", ev.ReactionType);
                        playAnimation("Hurt", "");
                        yield return new WaitForSeconds(1.0f);

                        if (battle.Witch.Health == 0)
                            playAnimation("Loss", "");
                    }
                    else
                    {
                        creatureElements[ev.Target].SetHealth(ev.Target.Health, ev.Target.MaxHealth);
                        creatureElements[ev.Target].setNumbersReceived(ev.Damage, ev.Element);
                        if (ev.Target.Health == 0)
                        {
                            creatureElements[ev.Target].playAnimation("Dead", 99); //99 used for NON-Reactions
                            yield return new WaitForSeconds(2.0f);
                            creatureElements[ev.Target].gameObject.SetActive(false);
                            enemiesDefeated++;
                            CheckEnemiesDefeated(enemiesDefeated, creatureElements.Count);
                        }
                        else
                            creatureElements[ev.Target].playAnimation("Hurt", ev.ReactionType);
                    }
                    yield return new WaitForSeconds(2.0f);
                    break;
                case ShieldEvent ev:
                    // GET SHIELD
                    playerShield.Shield = battle.Witch.Shield;
                    playAnimation("Shield", ev.Shield.Element.ToString());
                    yield return new WaitForSeconds(2.0f);
                    break;
                case HealEvent ev:
                    // HEALING
                    playerHealthBar.Set(battle.Witch.Health, battle.Witch.MaxHealth);
                    setNumbersReceived(ev.LifeRestored, ev.Element, "Heal", ev.ReactionType);
                    playAnimation("Heal", ev.ReactionType.ToString());
                    break;
                case BlockEvent ev:
                    // SHIELD BLOCK/BREAK
                    Debug.Log($"[DEBUG] Blocked {battle.Witch.Shield.Element}!");
                    logText = $"Blocked with {battle.Witch.Shield.Element} shield!";
                    if (battle.Witch.Shield.Charges == 0)
                    {
                        playAnimation("Break", battle.Witch.Shield.Element.ToString());
                    }
                    else
                    {
                        playAnimation("Block", battle.Witch.Shield.Element.ToString());
                    }
                    
                    playerShield.Shield = battle.Witch.Shield;
                    yield return new WaitForSeconds(2.0f);
                    break;
                case EmptyEvent ev:
                    Debug.Log($"[DEBUG] {ev.Warning}");
                    yield return new WaitForSeconds(2.0f);
                    break;
                default:
                    Debug.Log($"[DEBUG] {battleEvent.GetType()}");
                    break;
            }
        }
    }

    private void ShowHand(Battle battle)
    {
        for (int i = 0; i < cardContainer.transform.childCount; i++)
        {
            Transform cardButton = cardContainer.transform.GetChild(i);
            if (i < battle.Witch.Hand.Count && battle.Witch.Hand[i].Type != CardType.None)
            {
                cardButton.gameObject.SetActive(true);
                Card c = battle.Witch.Hand[i];
                UICardCreation uicard = cardButton.GetComponent<UICardCreation>();
                uicard.Create(c);
            }
            else
            {
                cardButton.gameObject.SetActive(false);
            }
        }
        drawPileTotalText.text = $"{battle.Witch.Deck.Count}";
        discardPileTotalText.text = $"{battle.Witch.DiscardPile.Count}";
    }

    private void ToggleCards(bool state)
    {
        for (int i = 0; i < cardContainer.transform.childCount; i++)
        {
            Transform card = cardContainer.transform.GetChild(i);
            Button b = card.GetComponent<Button>();
            b.interactable = state;
        }
    }

    private void ToggleTargets(bool state)
    {
        foreach (KeyValuePair<Battler, UICreature> item in creatureElements)
        {
            item.Value.TargetButton.interactable = state;
        }
    }

    private void HandleCardClick(int index, Button cardButton)
    {
        CloseActiveDialog(index, false);

        RectTransform cardTransform = cardButton.GetComponent<RectTransform>();
        activeCardActionDialog = Instantiate(cardActionDialogPrefab, cardTransform.position,
            Quaternion.identity, transform);

        activeCardActionDialog.OnPlay += () => HandleSelection(index);
        activeCardActionDialog.OnHold += () => HandleHoldCard(index);
        activeCardActionDialog.OnClose += () => CloseActiveDialog(index, true);
    }

    private void HandleSelection(int index)
    {
        CloseActiveDialog(index, false);

        for (int i = 0; i < battle.Creatures.Count; i++)
        {
            Creature c = battle.Creatures[i];
            Card card = battle.Witch.Hand[index];
            Attack attack = new Attack(card.Power, card.Element, new string[] {});
            (int damage, int reactionType) = c.GetDamageTaken(attack);
            creatureElements[c].setNumbersReceived(damage, attack.Element);
        }

        input = new InputResponse(Intention.Play, index);
        // Debug.Log("Index .play: " + index);
    }

    private void HandleHoldCard(int index)
    {
        CloseActiveDialog(index, false);
        Animator anim = cardContainer.transform.GetChild(index).GetComponent<Animator>();
        anim.SetTrigger("pHold");
        input = new InputResponse(Intention.Hold, index);
        Debug.Log("Index .hold: " + index);
    }

    private void CloseActiveDialog(int index, bool animation = true)
    {
        if (activeCardActionDialog != null)
        {
            Destroy(activeCardActionDialog.gameObject);
            if (animation == true)
            {
                Animator anim = cardContainer.transform.GetChild(index).GetComponent<Animator>();
                anim.SetTrigger("pNormal");
            }
        }
    }

    private void HandleEndTurnClick()
    {
        input = new InputResponse(Intention.EndTurn);
        playAnimation("Turn", "");
    }

    private void Update()
    {
        if (Input.GetButtonDown("[Cheat] Infinite Health"))
        {
            battle.Cheats[(int)Cheats.InfiniteHealth] = !battle.Cheats[(int)Cheats.InfiniteHealth];
            infiniteHealthText.text = $"Infinite Health: {battle.Cheats[(int)Cheats.InfiniteHealth]}";
            logText = $"Infinite Health is {battle.Cheats[(int)Cheats.InfiniteHealth]}";
        }

        if (Input.GetButtonDown("[Cheat] Infinite Damage"))
        {
            battle.Cheats[(int)Cheats.InfiniteDamage] = !battle.Cheats[(int)Cheats.InfiniteDamage];
            Debug.Log($"Infinite damage: battle.Cheats[(int)Cheats.InfiniteDamage]");
        }
    }

    private void CheckEnemiesDefeated(int defeated, int total)
    {
        if (defeated == total)
        {
            playAnimation("Win", "");
            PlayerResources pr = PlayerResources.Instance;
            pr.Gold = pr.Gold + 42;
            pr.SetStones(Element.Fire, pr.GetStones(Element.Fire) + 2);
            pr.SetStones(Element.Water, pr.GetStones(Element.Water) + 1);
            pr.SetStones(Element.Grass, pr.GetStones(Element.Grass) + 1);
        }
    }

    public void setNumbersReceived(int nreceived, Element element, string dmgHeal, int reactionType)
    {
        // Damage / Healing
        if (element.ToString() == "Fire")
            NmbReceived.color = Color.red;
        if (element.ToString() == "Water")
            NmbReceived.color = Color.cyan;
        if (element.ToString() == "Grass")
            NmbReceived.color = Color.green;

        if (dmgHeal == "Damage")
            NmbReceived.SetText("-" + nreceived.ToString());
        else     
            NmbReceived.SetText("+" + nreceived.ToString());

        Debug.Log(dmgHeal+ ": "+reactionType);
    }

    public void playAnimation(string animString, string extra)
    {
        if (extra != "")
        {
            if (extra == "Fire")
                animationShield.color = Color.red;
            if (extra == "Water")
                animationShield.color = Color.cyan;
            if (extra == "Grass")
                animationShield.color = Color.green;
        }

        if (animString == "Heal")
        {
            if (extra == "-1")
                reactions.text = "Weak..";
            else
                reactions.text = "Strong!";

            animString += extra;
            //Debug.Log("AAAAAAAAA - " + animString);
        }

        Animator.SetTrigger(animString);
    }
}
