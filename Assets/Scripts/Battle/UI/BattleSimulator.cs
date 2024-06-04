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
    [SerializeField] private Creature3D playerSprite;
    [SerializeField] private Animator shieldAnimator;
    [SerializeField] private Animator healAnimator;
    [SerializeField] private ParticleSystem healFire;
    [SerializeField] private ParticleSystem healWater;
    [SerializeField] private ParticleSystem healGrass;
    [SerializeField] private PlayerDropArea playerDropArea;
    [SerializeField] private HoldDropArea holdDropArea;
    [SerializeField] private FallbackDropArea fallbackDropArea;
    [SerializeField] private CardCounter playedCardsCounter;
    [SerializeField] private LootWindow lootWindow;

    //Audio
    [SerializeField] private AudioClip[] damageSoundClips;
    [SerializeField] private AudioClip[] healSoundClips;
    [SerializeField] private AudioClip[] shieldSoundClips;
    [SerializeField] private AudioClip[] blockSoundClips;
    [SerializeField] private AudioClip[] dissolveSoundClips;
  

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

    private List<(Item, int)> lootObtained;

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

            var uiCard = cardObject.GetComponent<UICardCreation>();
            uiCard.SetCancelEventListener(() => HandleCancelClick(iCopy, uiCard));

            var battleCard = cardObject.GetComponent<BattleCard>();
            battleCard.Index = iCopy;
            battleCard.OnCardBeginDrag += HandleCardBeginDrag;
            battleCard.OnCardEndDrag += HandleCardEndDrag;
        }

        ToggleCards(false);

        endTurnButton.onClick.AddListener(HandleEndTurnClick);
        endTurnButton.interactable = false;

        playerShield.Shield = new Shield();

        mainCamera = Camera.main;

        playerDropArea.OnPlayerTarget += HandlePlayerTarget;
        playerDropArea.gameObject.SetActive(false);

        holdDropArea.OnCardHold += HandleCardHold;
        holdDropArea.gameObject.SetActive(false);

        fallbackDropArea.OnCardDrop += HandleCardFallbackDrop;
        fallbackDropArea.gameObject.SetActive(false);

        lootObtained = new List<(Item, int)>();

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
            creatureElements[c].TargetButton.interactable = false;

            var dropArea = creatureElements[c].GetComponent<CreatureDropArea>();
            dropArea.Index = i;
            dropArea.OnCreatureTarget += HandleCreatureTarget;
            dropArea.enabled = false;

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

        var creatureSources = new Dictionary<Battler, EnemyCreature>();

        foreach (EncounterData encounter in request.encounters)
        {
            IList<Creature> creatures = new List<Creature>();
            foreach (EnemyCreature c in encounter.enemies)
            {
                Creature dummy = new Dummy("Dummy", c.health, c.element,
                    c.attackMin, c.attackMax);
                creatures.Add(dummy);
                creatureSources[dummy] = c;
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

            yield return RunBattle(battle, counter >= request.encounters.Length - 1, creatureSources);
            counter += 1;
        }

        FinishRequest();
    }

    private IEnumerator RunBattle(Battle battle, bool last,
        IDictionary<Battler, EnemyCreature> creatureSources)
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
                        if (battle.Witch.CardsPlayed >= 1)
                        {
                            endTurnButton.interactable = true;
                        }
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
                    Transform cardTransform = cardContainer.transform.GetChild(ev.Index);

                    //play soundfx
                    SoundFXManager.instance.PlayRandomSoundFXClip(dissolveSoundClips, transform, 0.5f);

                    Animator anima = cardTransform.GetComponent<Animator>();
                    anima.SetTrigger("Played");

                    UICardCreation uiCard = cardTransform.GetComponent<UICardCreation>();
                    uiCard.ToggleCancelButton(false);
                    uiCard.changeMaterialEdge(0.1f);

                    yield return new WaitForSeconds(1f);

                    uiCard.changeMaterialEdge(0f);
                    anima.ResetTrigger("Played");

                    playedCardsCounter.Set(battle.Witch.CardsPlayed);

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
                        playerHealthBar.Set(battle.Witch.Health, battle.Witch.MaxHealth);
                        setNumbersReceived(ev.Damage, ev.Element, "Damage", ev.ReactionType);
                        PlayAnimation("Hurt", "", "");                        
                        //play soundFXs
                        SoundFXManager.instance.PlayRandomSoundFXClip(damageSoundClips, transform, 1f);
                        yield return new WaitForSeconds(1.0f);

                        if (battle.Witch.Health == 0)
                            PlayAnimation("Loss", "", "");
                    }
                    else
                    {

                        Animator[] EnemyAllAnimators;
                        EnemyAllAnimators = creature3dElements[ev.Target].GetComponentsInChildren<Animator>();
                        // Attack animations
                        if (ev.DamageTag == "melee")
                        {
                            if (ev.Element.ToString() == "Fire")
                                EnemyAllAnimators[1].SetTrigger("FireSlash");
                            else if (ev.Element.ToString() == "Water")
                                EnemyAllAnimators[1].SetTrigger("WaterSlash");
                            else if (ev.Element.ToString() == "Grass")
                                EnemyAllAnimators[1].SetTrigger("GrassSlash");
                            
                            //play soundFXs
                            SoundFXManager.instance.PlayRandomSoundFXClip(damageSoundClips, transform, 1f);
                        }
                        else if (ev.DamageTag == "ranged")
                        {
                            if (ev.Element.ToString() == "Fire")
                                EnemyAllAnimators[1].SetTrigger("FireExplosion");
                            else if (ev.Element.ToString() == "Water")
                                EnemyAllAnimators[1].SetTrigger("WaterExplosion");
                            else if (ev.Element.ToString() == "Grass")
                                EnemyAllAnimators[1].SetTrigger("GrassExplosion");

                            //play soundFXs
                            SoundFXManager.instance.PlayRandomSoundFXClip(damageSoundClips, transform, 1f);
                        }
                        
                        creatureElements[ev.Target].SetHealth(ev.Target.Health, ev.Target.MaxHealth);
                        creatureElements[ev.Target].setNumbersReceived(ev.Damage, ev.Element);
                        if (ev.Target.Health == 0)
                        {
                            creatureElements[ev.Target].PlayAnimation("Dead", 99); //99 used for NON-Reactions
                            creature3dElements[ev.Target].PlayAnimation("Dead");

                            EnemyCreature creature = creatureSources[ev.Target];
                            (Item item, int amount) = BattleSettings.Instance.LootTable.RollLoot(creature);
                            lootObtained.Add((item, amount));

                            (Item goldItem, int gold) = BattleSettings.Instance.LootTable.GetGold(creature);
                            lootObtained.Add((goldItem, gold));

                            yield return new WaitForSeconds(2f);

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
                    SoundFXManager.instance.PlayRandomSoundFXClip(shieldSoundClips, transform, 1f);
                    PlayAnimation("get", ev.Shield.Element.ToString(), "shield");
                    yield return new WaitForSeconds(1.0f);

                    break;
                case HealEvent ev:
                    // HEALING
                    playerHealthBar.Set(battle.Witch.Health, battle.Witch.MaxHealth);
                    setNumbersReceived(ev.LifeRestored, ev.Element, "Heal", ev.ReactionType);
                    SoundFXManager.instance.PlayRandomSoundFXClip(healSoundClips, transform, 1f);
                    PlayAnimation("Heal", ev.Element.ToString(), ev.ReactionType.ToString());
                    break;
                case BlockEvent ev:
                    // SHIELD BLOCK/BREAK
                    Debug.Log($"[DEBUG] Blocked {battle.Witch.Shield.Element}!");
                    logText = $"Blocked with {battle.Witch.Shield.Element} shield!";
                    SoundFXManager.instance.PlayRandomSoundFXClip(blockSoundClips, transform, 1f);
                    if (battle.Witch.Shield.Charges == 0)
                    {
                        PlayAnimation("lose", battle.Witch.Shield.Element.ToString(), "shield");
                        yield return new WaitForSeconds(1f);
                        shieldAnimator.ResetTrigger("getGrass");
                        shieldAnimator.ResetTrigger("getFire");
                        //shieldAnimator.ResetTrigger("getWater");
                    }
                    else
                    {
                        PlayAnimation("block", battle.Witch.Shield.Element.ToString(), "shield");
                    }

                    playerShield.Shield = battle.Witch.Shield;
                    break;
                case EndTurnEvent ev:
                {
                    playedCardsCounter.Set(0);
                    break;
                }
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

            var canvasGroup = card.GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = state;
        }
    }

    private void ToggleTargets(bool state)
    {
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
        PlayAnimation("Win", "", "");
        lootWindow.Loot = lootObtained;

        PlayerResources pr = PlayerResources.Instance;
        foreach ((Item item, int amount) in lootObtained)
        {
            if (item is GoldItem gold)
            {
                pr.Gold = pr.Gold + amount;
                Debug.Log($"Gained {amount} gold (total: {pr.Gold})");
            }
            else
            {
                for (int i = 0; i < amount; i++)
                {
                    pr.Obtain(item);
                }
            }
        }
        lootObtained.Clear();

        BattleSettings bs = BattleSettings.Instance;
        bs.NextStage();
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
        if (reaction == "shield")
        {
            string shieldString;
            shieldString = animString + element;
            //Debug.Log("REEE: " + shieldString);

            shieldAnimator.SetTrigger(shieldString);    
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

    private void HandleCardBeginDrag(BattleCard battleCard)
    {
        Card card = battleCard.CurrentCard;

        holdDropArea.gameObject.SetActive(true);
        fallbackDropArea.gameObject.SetActive(true);

        if (battle.Witch.CardsPlayed >= 2)
        {
            return;
        }

        if (card.Type == CardType.Sword || card.Type == CardType.Spell)
        {
            for (int i = 0; i < battle.Creatures.Count; i++)
            {
                if (battle.Creatures[i].Health > 0)
                {
                    creatureElements[battle.Creatures[i]].GetComponent<CreatureDropArea>().enabled = true;
                    creature3dElements[battle.Creatures[i]].ToggleTarget(true);

                    Attack attack = new Attack(card.Power, card.Element, new string[] {""});
                    (int damage, int reactionType) = battle.Creatures[i].GetDamageTaken(attack);
                    creatureElements[battle.Creatures[i]].setNumbersReceived(damage, attack.Element);

                    creatureElements[battle.Creatures[i]].TargetButton.interactable = true;
                }
            }
        }
        else if (card.Type == CardType.Shield || card.Type == CardType.Heal)
        {
            playerDropArea.gameObject.SetActive(true);
            if (playerSprite != null)
            {
                playerSprite.ToggleTarget(true);
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

    private void HandlePlayerTarget(BattleCard battleCard)
    {
        HandleSelection(battleCard.Index);

        Debug.Log($"Dropped {battleCard.CurrentCard} on player");
    }

    private void HandleCardHold(BattleCard battleCard)
    {
        HandleHoldCard(battleCard.Index);
    }

    private void HandleCardFallbackDrop(BattleCard battleCard)
    {
        int index = battleCard.Index;

        if (battle.Witch.HeldCards.Contains(index))
        {
            input = new InputResponse(Intention.Unhold, index);
            Animator animator = cardContainer.transform.GetChild(index).GetComponent<Animator>();
            animator.SetTrigger("pNormal");
        }
    }

    private void HandleCardEndDrag(BattleCard battleCard)
    {
        foreach (Creature c in battle.Creatures)
        {
            creature3dElements[c].ToggleTarget(false);
            creatureElements[c].TargetButton.interactable = false;
            creatureElements[c].GetComponent<CreatureDropArea>().enabled = false;
        }

        playerSprite.ToggleTarget(false);

        playerDropArea.gameObject.SetActive(false);
        holdDropArea.gameObject.SetActive(false);
        fallbackDropArea.gameObject.SetActive(false);
    }
}
