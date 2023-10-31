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
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private GameObject creatureContainer;

    private IDictionary<Battler, UICreature> creatureElements;

    private int input = -1;

    private void Start()
    {
        creatureElements = new Dictionary<Battler, UICreature>();

        IList<Card> cards = new List<Card>
        {
            new Card(CardType.Sword, Element.Fire, 3),
            new Card(CardType.Spell, Element.Water, 2),
            new Card(CardType.Spell, Element.Grass, 2),
            new Card(CardType.Heal, Element.Fire, 3)
        };

        Witch witch = new Witch("Witch", 20, cards, 5, 4);

        IList<Creature> creatures = new List<Creature>
        {
            new Dummy("Dummy", 10, Element.Grass)
        };

        ILogger logger = new UnityLogger();

        Battle battle = new Battle(witch, creatures, logger);

        for (int i = 0; i < battle.Creatures.Count; i++)
        {
            Battler c = battle.Creatures[i];
            Transform cc = creatureContainer.transform.GetChild(i);
            creatureElements[c] = cc.GetComponent<UICreature>();
        }

        for (int i = 0; i < cardContainer.transform.childCount; i++)
        {
            int iCopy = i;
            Button b = cardContainer.transform.GetChild(i).GetComponent<Button>();
            b.onClick.AddListener(() => HandleCardSelect(iCopy));
        }
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
                    }
                    else if (ev.Type == InputRequestType.Target)
                    {
                        Debug.Log("[DEBUG] Choose a target");
                    }
                    input = -1;
                    yield return new WaitUntil(() => input != -1);
                    battle.Witch.Input = input;
                    break;
                case DrawEvent ev:
                    ShowHand(battle);
                    yield return new WaitForSeconds(0.5f);
                    break;
                case CardEvent ev:
                    Debug.Log($"[DEBUG] Played {ev.Card}");
                    ShowHand(battle);
                    break;
                case DamageEvent ev:
                    if (ev.Target == battle.Witch)
                    {
                        playerHealthBar.value = (float)battle.Witch.Health / battle.Witch.MaxHealth;
                    }
                    else
                    {
                        float health = (float)ev.Target.Health / ev.Target.MaxHealth;
                        creatureElements[ev.Target].Health = health;
                    }
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
            if (i < battle.Witch.Hand.Count)
            {
                cardButton.gameObject.SetActive(true);
                Card c = battle.Witch.Hand[i];
                cardButton.GetComponentInChildren<TMP_Text>().text = c.ToString();
            }
            else
            {
                cardButton.gameObject.SetActive(false);
            }
        }
    }

    private void HandleCardSelect(int index)
    {
        input = index;
    }
}
