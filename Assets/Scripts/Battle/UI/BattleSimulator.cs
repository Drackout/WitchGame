using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private GameObject creature3dContainer;
    [SerializeField] private GameObject creature3dPrefab;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TMP_Text infiniteHealthText;
    [SerializeField] private TMP_Text drawPileTotalText;
    [SerializeField] private TMP_Text discardPileTotalText;
    [SerializeField] private TMP_Text NmbReceived;
    [SerializeField] private TMP_Text reactions;
    [SerializeField] private CardActionsDialog cardActionDialogPrefab;
    [SerializeField] private BattleLog battleLogger;
    [SerializeField] private UISlots slots;
    [SerializeField] private Image animationShield;
    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private AudioSource audioSrc2;
    [SerializeField] private LayerMask enemiesMask;
    [SerializeField] private Sprite waterShield;
    [SerializeField] private Sprite grassShield;
    [SerializeField] private Sprite fireShield;

    private Battle battle;
    private IDictionary<Battler, UICreature> creatureElements;
    private IDictionary<Battler, Creature3D> creature3dElements;

    private RequestData request;
    private int encounterIndex;

    private InputResponse input;

    private CardActionsDialog activeCardActionDialog;

    private string logText;

    private int enemiesDefeated;

    private Animator Animator;

    private CardAnimation cardAnimation;

    private Camera mainCamera;

    private bool selectingTarget;

    private void Start()
    {
        creatureElements = new Dictionary<Battler, UICreature>();
        creature3dElements = new Dictionary<Battler, Creature3D>();

        System.Random rnd = new System.Random();
        Animator = gameObject.GetComponentInChildren<Animator>();
        enemiesDefeated = 0;

        encounterIndex = 0;

        BattleSettings battleSettings = BattleSettings.Instance;

        for (int i = 0; i < cardContainer.transform.childCount; i++)
        {
            int iCopy = i;
            Button b = cardContainer.transform.GetChild(i).GetComponent<Button>();
            b.onClick.AddListener(() => HandleCardClick(iCopy, b));
            b.interactable = false;
        }

        endTurnButton.onClick.AddListener(HandleEndTurnClick);
        endTurnButton.interactable = false;

        cancelButton.onClick.AddListener(HandleCancelClick);
        cancelButton.gameObject.SetActive(false);

        playerShield.Shield = new Shield();

        mainCamera = Camera.main;

        StartCoroutine(RunRequest());
    }

    private void InitCreatures()
    {
        BattleSettings battleSettings = BattleSettings.Instance;

        for (int i = 0; i < battle.Creatures.Count; i++)
        {
            int iCopy = i;
            Creature c = battle.Creatures[i];

            Transform slot = creatureContainer.transform.GetChild(i);
            EnemyCreature creatureData = request.encounters[encounterIndex].enemies[i];
            UICreature uiCreature = Instantiate(creatureData.prefab, slot);

            creatureElements[c] = uiCreature;
            creatureElements[c].SetHealth(c.Health, c.MaxHealth);
            creatureElements[c].Element = c.Element;
            creatureElements[c].TargetButton.onClick.AddListener(
                () => HandleSelection(iCopy));

            Transform creature3dSlot = creature3dContainer.transform.GetChild(i);
            GameObject creature3d = Instantiate(creatureData.meshPrefab, creature3dSlot);

            creature3dElements[c] = creature3d.GetComponent<Creature3D>();
        }
    }

    private IEnumerator RunRequest()
    {
        request = BattleSettings.Instance.CurrentRequest;

        IList<Card> cards = new List<Card>(PlayerResources.Instance.Decks[0]);
        Witch witch = new Witch("Witch", 20, cards, 5, 4, 3);

        foreach (EncounterData encounter in request.encounters)
        {
            IList<Creature> creatures = new List<Creature>();
            foreach (EnemyCreature c in request.encounters[encounterIndex].enemies)
            {
                creatures.Add(new Dummy("Dummy", c.health, c.element));
            }

            battle = new Battle(witch, creatures, battleLogger);

            InitCreatures();

            slots.Slots = battle.Witch.Slots;
            playerHealthBar.Set(battle.Witch.Health, battle.Witch.MaxHealth);

            yield return RunBattle(battle);
        }

        FinishRequest();
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
                        if (input.Intention == Intention.Cancel)
                        {
                            for (int i = 0; i < cardContainer.transform.childCount; i++)
                            {
                                Animator anim = cardContainer.transform.GetChild(i)
                                    .GetComponent<Animator>();
                                anim.SetTrigger("pNormal");
                            }
                        }
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
                        {
                            creatureElements[battle.Creatures[i]].PlayAnimation("Attack", 99);
                            creature3dElements[battle.Creatures[i]].PlayAnimation("Attack");
                        }
                    }
                    yield return new WaitForSeconds(0.5f);
                    break;
                case PlayCardEvent ev:
                    // yield return new WaitForSeconds(1f);  // time with card dissolve
                    Debug.Log($"[DEBUG] Played {ev.Card}");
                    Debug.Log($"Element: {ev.Card.Element}");
                    logText = $"Played {ev.Card}";
                    // Material cardPlayerMat = ev.Card.ima ent<Material>()
                    ShowHand(battle);
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
                        PlayAnimation("Hurt", "");
                        yield return new WaitForSeconds(1.0f);

                        if (battle.Witch.Health == 0)
                            PlayAnimation("Loss", "");
                    }
                    else
                    {
                        creatureElements[ev.Target].SetHealth(ev.Target.Health, ev.Target.MaxHealth);
                        creatureElements[ev.Target].setNumbersReceived(ev.Damage, ev.Element);
                        if (ev.Target.Health == 0)
                        {
                            creatureElements[ev.Target].PlayAnimation("Dead", 99); //99 used for NON-Reactions
                            creature3dElements[ev.Target].PlayAnimation("Dead");
                            yield return new WaitForSeconds(2.0f);
                            creatureElements[ev.Target].gameObject.SetActive(false);
                            creature3dElements[ev.Target].gameObject.SetActive(false);
                            enemiesDefeated++;
                        }
                        else
                        {
                            creatureElements[ev.Target].PlayAnimation("Hurt", ev.ReactionType);
                            creature3dElements[ev.Target].PlayAnimation("Hurt");
                        }
                    }
                    yield return new WaitForSeconds(2.0f);
                    break;
                case ShieldEvent ev:
                    // GET SHIELD
                    playerShield.Shield = battle.Witch.Shield;
                    PlayAnimation("Shield", ev.Shield.Element.ToString());
                    yield return new WaitForSeconds(2.0f);
                    break;
                case HealEvent ev:
                    // HEALING
                    playerHealthBar.Set(battle.Witch.Health, battle.Witch.MaxHealth);
                    setNumbersReceived(ev.LifeRestored, ev.Element, "Heal", ev.ReactionType);
                    PlayAnimation("Heal", ev.ReactionType.ToString());
                    break;
                case BlockEvent ev:
                    // SHIELD BLOCK/BREAK
                    Debug.Log($"[DEBUG] Blocked {battle.Witch.Shield.Element}!");
                    logText = $"Blocked with {battle.Witch.Shield.Element} shield!";
                    if (battle.Witch.Shield.Charges == 0)
                    {
                        PlayAnimation("Break", battle.Witch.Shield.Element.ToString());
                    }
                    else
                    {
                        PlayAnimation("Block", battle.Witch.Shield.Element.ToString());
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

        foreach (Creature3D c in creature3dElements.Values)
        {
            c.ToggleTarget(state);
        }

        cancelButton.gameObject.SetActive(state);

        selectingTarget = state;
    }

    private void HandleCardClick(int index, Button cardButton)
    {
        if (battle.Witch.HeldCards.Contains(index))
        {
            input = new InputResponse(Intention.Unhold, index);
            Animator animator = cardContainer.transform.GetChild(index).GetComponent<Animator>();
            animator.SetTrigger("pNormal");
        }
        else
        {
            CloseActiveDialog(index, false);

            RectTransform cardTransform = cardButton.GetComponent<RectTransform>();
            activeCardActionDialog = Instantiate(cardActionDialogPrefab, cardTransform.position,
                Quaternion.identity, transform);

            activeCardActionDialog.OnPlay += () => HandleSelection(index);
            activeCardActionDialog.OnHold += () => HandleHoldCard(index);
            activeCardActionDialog.OnClose += () => CloseActiveDialog(index, true);
        }
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

            //Animator anim = cardContainer.transform.GetChild(index).GetComponent<Animator>();
            //anim.SetTrigger("Played");
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
        PlayAnimation("Turn", "");
    }

    private void HandleCancelClick()
    {
        input = new InputResponse(Intention.Cancel);
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

        if (selectingTarget)
        {
            foreach (Creature3D c in creature3dElements.Values)
            {
                c.ToggleHover(false);
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10f, enemiesMask))
            {
                var creature = hit.collider.GetComponent<Creature3D>();
                creature.ToggleHover(true);
            }
        }
    }

    private bool CheckEnemiesDefeated(int defeated, int total)
    {
        return defeated == total;
    }

    private void FinishRequest()
    {
        encounterIndex = 0;
        BattleSettings bs = BattleSettings.Instance;
        bs.NextStage();

        PlayAnimation("Win", "");
        PlayerResources pr = PlayerResources.Instance;
        pr.Gold = pr.Gold + 42;
        pr.SetStones(Element.Fire, pr.GetStones(Element.Fire) + 2);
        pr.SetStones(Element.Water, pr.GetStones(Element.Water) + 1);
        pr.SetStones(Element.Grass, pr.GetStones(Element.Grass) + 1);
        pr.NeutralCards.Add(new Card(CardType.Spell, Element.None, 2));
        pr.NeutralCards.Add(new Card(CardType.Heal, Element.None, 1));
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

    public void PlayAnimation(string animString, string extra)
    {
        if (extra != "")
        {
            if (extra == "Fire")
                animationShield.sprite = fireShield;
            if (extra == "Water")
                animationShield.sprite = waterShield;
            if (extra == "Grass")
                animationShield.sprite = grassShield;
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


    public void playWinSound()
    {
        audioSrc.Play();
    }

    public void stopSounds()
    {
        audioSrc2.Stop();
    }

}
