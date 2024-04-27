using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using BattleEvents;
using TMPro;
using Unity.VisualScripting;

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
    [SerializeField] private Button cancelButtonPrefab;
    [SerializeField] private TMP_Text infiniteHealthText;
    [SerializeField] private TMP_Text drawPileTotalText;
    [SerializeField] private TMP_Text discardPileTotalText;
    [SerializeField] private TMP_Text NmbReceived;
    [SerializeField] private TMP_Text reactions;
    [SerializeField] private CardActionsDialog cardActionDialogPrefab;
    [SerializeField] private BattleLog battleLogger;
    [SerializeField] private UISlots slots;
    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private AudioSource audioSrc2;
    [SerializeField] private LayerMask enemiesMask;
    [SerializeField] private SpriteRenderer shieldMat;
    [SerializeField] private Material waterShieldMat;
    [SerializeField] private Material grassShieldMat;
    [SerializeField] private Material fireShieldMat;
    [SerializeField] private Animator PanelAnimator;
    [SerializeField] private Animator player3dAnimator;
    [SerializeField] private Animator shieldAnimator;
    [SerializeField] private Animator healAnimator;
    [SerializeField] private ParticleSystem healFire;
    [SerializeField] private ParticleSystem healWater;
    [SerializeField] private ParticleSystem healGrass;
    [SerializeField] private CreatureDropArea[] creatureDropAreas;

    private Battle battle;
    private IDictionary<Battler, UICreature> creatureElements;
    private IDictionary<Battler, Creature3D> creature3dElements;

    private RequestData request;

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

        BattleSettings battleSettings = BattleSettings.Instance;

        for (int i = 0; i < cardContainer.transform.childCount; i++)
        {
            int iCopy = i;

            Transform cardObject = cardContainer.transform.GetChild(i);

            Button b = cardObject.GetComponent<Button>();
            b.onClick.AddListener(() => HandleCardClick(iCopy, b));

            var uiCard = cardObject.GetComponent<UICardCreation>();
            uiCard.SetCancelEventListener(() => HandleCancelClick(iCopy, uiCard));

            var battleCard = cardObject.GetComponent<BattleCard>();
            battleCard.Index = iCopy;
            battleCard.OnCardBeginDrag += HandleCardBeginDrag;
            battleCard.OnCardEndDrag += HandleCardEndDrag;

            b.interactable = false;
        }

        endTurnButton.onClick.AddListener(HandleEndTurnClick);
        endTurnButton.interactable = false;

        playerShield.Shield = new Shield();

        mainCamera = Camera.main;

        foreach (CreatureDropArea area in creatureDropAreas)
        {
            area.OnCreatureTarget += HandleCreatureTarget;
            area.gameObject.SetActive(false);
        }

        StartCoroutine(RunRequest());
    }

    private void InitCreatures(EncounterData encounter)
    {
        BattleSettings battleSettings = BattleSettings.Instance;

        for (int i = 0; i < battle.Creatures.Count; i++)
        {
            int iCopy = i;
            Creature c = battle.Creatures[i];

            Transform slot = creatureContainer.transform.GetChild(i);
            EnemyCreature creatureData = encounter.enemies[i];
            UICreature uiCreature = Instantiate(creatureData.prefab, slot);

            // Draw creatures behind drop areas
            uiCreature.transform.SetSiblingIndex(0);

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

        int counter = 0;

        foreach (EncounterData encounter in request.encounters)
        {
            IList<Creature> creatures = new List<Creature>();
            foreach (EnemyCreature c in encounter.enemies)
            {
                Creature dummy = new Dummy("Dummy", c.health, c.element,
                    c.attackMin, c.attackMax);
                creatures.Add(dummy);
            }

            ElementConfig elementConfig = BattleSettings.Instance.ElementConfig;

            battle = new Battle(witch, creatures, battleLogger, elementConfig.ElementTable);
            battle.SetModifiers(elementConfig.DamagePositiveMod,
                elementConfig.DamageNegativeMod,
                elementConfig.HealPositiveMod,
                elementConfig.HealNegativeMod);

            InitCreatures(encounter);

            slots.Slots = battle.Witch.Slots;
            playerHealthBar.Set(battle.Witch.Health, battle.Witch.MaxHealth);

            yield return RunBattle(battle, counter >= request.encounters.Length - 1);
            counter += 1;
        }

        FinishRequest();
    }

    private IEnumerator RunBattle(Battle battle, bool last)
    {
        IEnumerable<BattleEvent> battleIter = battle.Run();

        foreach (BattleEvent battleEvent in battleIter)
        {
            switch (battleEvent)
            {
                case InputRequestEvent ev:
                    if (ev.Type == InputRequestType.Play)
                    {
                        ToggleCards(true);
                        endTurnButton.interactable = true;
                    }
                    else if (ev.Type == InputRequestType.Target)
                    {
                        ToggleTargets(true);
                    }
                    input = new InputResponse();
                    yield return new WaitUntil(() => input.Intention != Intention.None);
                    battle.Witch.Input = input;
                    if (ev.Type == InputRequestType.Play)
                    {
                        ToggleCards(false);
                        endTurnButton.interactable = false;
                        if (input.Intention == Intention.Play)
                        {
                            Debug.Log($"Playing card {input.Selection}");
                            int selectedCard = input.Selection;
                            UICardCreation uiCard = cardContainer.transform
                                .GetChild(input.Selection).GetComponent<UICardCreation>();
                            uiCard.ToggleCancelButton(true);
                        }
                    }
                    else if (ev.Type == InputRequestType.Target)
                    {
                        ToggleTargets(false);
                        if (input.Intention == Intention.Cancel)
                        {
                            Animator anim = cardContainer.transform.GetChild(input.Selection)
                                .GetComponent<Animator>();
                            anim.SetTrigger("pNormal");
                        }
                    }
                    break;
                case DrawEvent ev:
                    ShowHand(battle);
                    Slider sliders;
                    //Debug.Log("Aaa: " + ev);

                    //Set every Card slider to value 0 (for dissolve)
                    for (int i = 0; i < cardContainer.transform.childCount; i++)
                    {
                        //Debug.Log("Card: " + i);
                        sliders = cardContainer.transform.GetChild(i).GetComponentInChildren<Slider>();
                        sliders.value = 0;
                    }
                    yield return new WaitForSeconds(0.5f);
                    break;
                case MoveEvent ev:
                    // ENEMY TURN (1 by 1)
                    PanelAnimator.ResetTrigger("PanelShow");
                    PanelAnimator.SetTrigger("PanelHide");
                    for (int i = 0; i < battle.Creatures.Count; i++)
                    {
                        Debug.Log("ev.battler1 " + ev.Battler);
                        if (battle.Creatures[i] == ev.Battler)
                        {
                            creatureElements[battle.Creatures[i]].PlayAnimation("Attack", 99);
                            creature3dElements[battle.Creatures[i]].PlayAnimation("Attack");
                        }
                    }
                    yield return new WaitForSeconds(1.5f);
                    break;
                case PlayCardEvent ev:
                {
                    Debug.Log("PLAYING CARD!!: "); /////// DO the card dissolve borders next.. 
                    // Change the 1 for the played card ID

                    Transform cardTransform = cardContainer.transform.GetChild(ev.Index);

                    Animator anima = cardTransform.GetComponent<Animator>();
                    anima.SetTrigger("Played");

                    UICardCreation uiCard = cardTransform.GetComponent<UICardCreation>();
                    uiCard.ToggleCancelButton(false);
                    uiCard.changeMaterialEdge(0.1f);

                    yield return new WaitForSeconds(1f);

                    uiCard.changeMaterialEdge(0f);
                    anima.ResetTrigger("Played");

                    Debug.Log($"[DEBUG] Played {ev.Card}");
                    //Debug.Log($"Element: {ev.Card.Element}");
                    logText = $"Played {ev.Card}";
                    // Material cardPlayerMat = ev.Card.ima ent<Material>()
                    ShowHand(battle);
                    break;
                }
                case HoldEvent ev:
                {
                    Animator anim = cardContainer.transform.GetChild(ev.HeldCard)
                        .GetComponent<Animator>();
                    if (ev.Success)
                    {
                        anim.SetTrigger("pHold");
                    }
                    else
                    {
                        anim.SetTrigger("pNormal");
                    }
                }
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
                        PlayAnimation("Hurt", "", "");
                        yield return new WaitForSeconds(1.0f);

                        if (battle.Witch.Health == 0)
                            PlayAnimation("Loss", "", "");
                    }
                    else
                    {
                        creatureElements[ev.Target].SetHealth(ev.Target.Health, ev.Target.MaxHealth);
                        creatureElements[ev.Target].setNumbersReceived(ev.Damage, ev.Element);
                        if (ev.Target.Health == 0)
                        {
                            creatureElements[ev.Target].PlayAnimation("Dead", 99); //99 used for NON-Reactions
                            creature3dElements[ev.Target].PlayAnimation("Dead");
                            yield return new WaitForSeconds(1.5f);
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
                    break;
                case ShieldEvent ev:
                    // GET SHIELD
                    playerShield.Shield = battle.Witch.Shield;
                    PlayAnimation("getShield", ev.Shield.Element.ToString(), "");
                    yield return new WaitForSeconds(1.0f);

                    break;
                case HealEvent ev:
                    // HEALING
                    playerHealthBar.Set(battle.Witch.Health, battle.Witch.MaxHealth);
                    setNumbersReceived(ev.LifeRestored, ev.Element, "Heal", ev.ReactionType);
                    PlayAnimation("Heal", ev.Element.ToString(), ev.ReactionType.ToString());
                    break;
                case BlockEvent ev:
                    // SHIELD BLOCK/BREAK
                    Debug.Log($"[DEBUG] Blocked {battle.Witch.Shield.Element}!");
                    logText = $"Blocked with {battle.Witch.Shield.Element} shield!";
                    if (battle.Witch.Shield.Charges == 0)
                    {
                        PlayAnimation("loseShield", battle.Witch.Shield.Element.ToString(), "");
                        yield return new WaitForSeconds(1f);
                        shieldAnimator.ResetTrigger("getShield");
                    }
                    else
                    {
                        PlayAnimation("blockShield", battle.Witch.Shield.Element.ToString(), "");
                    }

                    playerShield.Shield = battle.Witch.Shield;
                    break;
                case EmptyEvent ev:
                    Debug.Log($"[DEBUG] {ev.Warning}");
                    break;
                default:
                    Debug.Log($"[DEBUG] {battleEvent.GetType()}");
                    break;
            }

            if (last && battle.IsOver())
            {
                break;
            }
        }
    }

    private void ShowHand(Battle battle)
    {
        // PanelAnimator.SetTrigger("PanelShow");
        //Debug.Log("ShowHand 1 ");
        for (int i = 0; i < cardContainer.transform.childCount; i++)
        {
            Transform cardButton = cardContainer.transform.GetChild(i);
            if (i < battle.Witch.Hand.Count && battle.Witch.Hand[i].Type != CardType.None)
            {
                int iCopy = i;
                cardButton.gameObject.SetActive(true);
                Card c = battle.Witch.Hand[i];
                UICardCreation uicard = cardButton.GetComponent<UICardCreation>();
                uicard.Create(c);
                BattleCard battleCard = cardButton.GetComponent<BattleCard>();
                battleCard.CurrentCard = c;

                // When all cards in hand put panel back up
                if (i+1 == 1)
                {
                    PanelAnimator.ResetTrigger("PanelHide");
                    PanelAnimator.SetTrigger("PanelShow");
                }
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

            // Force all cards down when clicking in another card, reset triggers
            for (int i = 0; i < cardContainer.transform.childCount; i++)
            {
                Animator anim = cardContainer.transform.GetChild(i).GetComponent<Animator>();
                if (!battle.Witch.HeldCards.Contains(i) && i != index)
                {
                    anim.ResetTrigger("pClick1");
                    anim.SetTrigger("pNormal");
                }
                else
                {
                    anim.ResetTrigger("pNormal");
                    anim.SetTrigger("pClick1");
                    Debug.Log($"Reset animation on card {index}");
                }
            }


            //Animator anim = cardContainer.transform.GetChild(index).GetComponent<Animator>();
            //anim.SetTrigger("pClick1");

            RectTransform cardTransform = cardButton.GetComponent<RectTransform>();
            activeCardActionDialog = Instantiate(cardActionDialogPrefab, cardTransform.position,
                Quaternion.identity, transform);

            activeCardActionDialog.OnPlay += () => HandleSelection(index);
            activeCardActionDialog.OnHold += () => HandleHoldCard(index);
            activeCardActionDialog.OnClose += () => CloseActiveDialog(index, true);
            Debug.Log("A2");
        }
    }

    private void HandleSelection(int index)
    {
        CloseActiveDialog(index, false);

        for (int i = 0; i < battle.Creatures.Count; i++)
        {
            Creature c = battle.Creatures[i];
            Card card = battle.Witch.Hand[index];

            // This method is used for both card and enemy selection.
            // Checking for type advantage if the selection is of an enemy
            // can create issues if the index is the same as a `None` card.
            //
            // Continue if index matches a `None` card as a temporary fix.
            if (card.Type == CardType.None)
            {
                continue;
            }

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
        PlayAnimation("Turn", "", "");
    }

    private void HandleCancelClick(int index, UICardCreation uiCard)
    {
        Debug.Log($"Canceled card {index}");
        uiCard.ToggleCancelButton(false);
        input = new InputResponse(Intention.Cancel, input.Selection);
        Animator anim = cardContainer.transform.GetChild(index).GetComponent<Animator>();
        anim.SetTrigger("pNormal");
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
        BattleSettings bs = BattleSettings.Instance;
        bs.NextStage();

        PlayAnimation("Win", "", "");
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

    public void PlayAnimation(string animString, string element, string reaction)
    {
        if (animString == "loseShield" || animString == "getShield" || animString == "blockShield")
        {
            if (element == "Fire")
                shieldMat.material = fireShieldMat;
            if (element == "Water")
                shieldMat.material = waterShieldMat;
            if (element == "Grass")
                shieldMat.material = grassShieldMat;
            shieldAnimator.SetTrigger(animString);
        }

        if (animString == "Heal")
        {
            //if (element == "Fire")
            //    healFire.Play(true);
            //if (element == "Water")
            //    healWater.Play(true);
            //if (element == "Grass")
            //    healGrass.Play(true);

            if (reaction == "-1")
            {
                reactions.text = "Weak..";
                if (element == "Fire")
                {
                    var emission = healFire.emission;
                    emission.rateOverTime = 4;
                }
                if (element == "Water")
                {
                    var emission = healWater.emission;
                    emission.rateOverTime = 4;
                }
                if (element == "Grass")
                {
                    var emission = healGrass.emission;
                    emission.rateOverTime = 4;
                }
            }
            else
            {
                reactions.text = "Strong!";
                if (element == "Fire")
                {
                    var emission = healFire.emission;
                    emission.rateOverTime = 10;
                }
                if (element == "Water")
                {
                    var emission = healWater.emission;
                    emission.rateOverTime = 10;
                }
                if (element == "Grass")
                {
                    var emission = healGrass.emission;
                    emission.rateOverTime = 10;
                }
            }

            animString += reaction;
            healAnimator.SetTrigger(element);
            //Debug.Log("AAAAAAAAA - " + animString);
        }

        Animator.SetTrigger(animString);
        player3dAnimator.SetTrigger(animString);
    }


    public void playWinSound()
    {
        audioSrc.Play();
    }

    public void stopSounds()
    {
        audioSrc2.Stop();
    }

    private void HandleCardBeginDrag(Card card)
    {
        if (card.Type == CardType.Sword || card.Type == CardType.Spell)
        {
            for (int i = 0; i < battle.Creatures.Count; i++)
            {
                if (battle.Creatures[i].Health > 0)
                {
                    creatureDropAreas[i].gameObject.SetActive(true);
                }
            }
        }
    }

    private IEnumerator SelectCardAndCreature(int cardIndex, int creatureIndex)
    {
        HandleSelection(cardIndex);
        yield return null;
        HandleSelection(creatureIndex);
    }

    private void HandleCreatureTarget(BattleCard battleCard, int creatureIndex)
    {
        StartCoroutine(SelectCardAndCreature(battleCard.Index, creatureIndex));

        Debug.Log(String.Format("Dropped {0} in slot {1} on enemy {2}",
            battleCard.CurrentCard,
            battleCard.Index,
            creatureIndex
        ));
    }

    private void HandleCardEndDrag(Card card)
    {
        foreach (CreatureDropArea area in creatureDropAreas)
        {
            area.gameObject.SetActive(false);
        }
    }
}
