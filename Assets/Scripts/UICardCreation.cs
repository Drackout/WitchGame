using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using BattleEvents;
using TMPro;

public class UICardCreation : MonoBehaviour
{
    [SerializeField] private GameObject fireBook;
    [SerializeField] private GameObject fireSword;
    [SerializeField] private GameObject fireHeal;
    [SerializeField] private GameObject fireShield;
    [SerializeField] private GameObject waterBook;
    [SerializeField] private GameObject waterSword;
    [SerializeField] private GameObject waterHeal;
    [SerializeField] private GameObject waterShield;
    [SerializeField] private GameObject grassBook;
    [SerializeField] private GameObject grassSword;
    [SerializeField] private GameObject grassHeal;
    [SerializeField] private GameObject grassShield;
    [SerializeField] private GameObject neutralBook;
    [SerializeField] private GameObject neutralSword;
    [SerializeField] private GameObject neutralHeal;
    [SerializeField] private GameObject neutralShield;

    [SerializeField] private TextMeshProUGUI powerNumber;
    public void Create(Card toShow)
    {
        switch(toShow.Type)
        {
            case CardType.Shield:
                if (toShow.Element == Element.Fire)
                {
                    Instantiate(fireShield, transform);
                }
                else if (toShow.Element == Element.Grass)
                {
                    Instantiate(grassShield, transform);
                }
                else if (toShow.Element == Element.Water)
                {
                    Instantiate(waterShield, transform);
                }
                else
                {
                    Instantiate(neutralShield, transform);
                }
                break;

            case CardType.Heal:
                if (toShow.Element == Element.Fire)
                {
                    Instantiate(fireShield, transform);
                }
                else if (toShow.Element == Element.Grass)
                {
                    Instantiate(grassHeal, transform);
                }
                else if (toShow.Element == Element.Water)
                {
                    Instantiate(waterHeal, transform);
                }
                else
                {
                    Instantiate(neutralHeal, transform);
                }
                break;

            case CardType.Sword:
                if (toShow.Element == Element.Fire)
                {
                    Instantiate(fireSword, transform);
                }
                else if (toShow.Element == Element.Grass)
                {
                    Instantiate(grassSword, transform);
                }
                else if (toShow.Element == Element.Water)
                {
                    Instantiate(waterSword, transform);
                }
                else
                {
                    Instantiate(neutralSword, transform);
                }
                break;

            case CardType.Spell:
                if (toShow.Element == Element.Fire)
                {
                    Instantiate(fireBook, transform);
                }
                else if (toShow.Element == Element.Grass)
                {
                    Instantiate(grassBook, transform);
                }
                else if (toShow.Element == Element.Water)
                {
                    Instantiate(waterBook, transform);
                }
                else
                {
                    Instantiate(neutralBook, transform);
                }
                break;
        }

        powerNumber.text = toShow.Power.ToString();

        /*for (int i = 0; i < cardContainer.transform.childCount; i++)
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
        drawPileTotalText.text = $"{battle.Witch.Deck.Count}";
        discardPileTotalText.text = $"{battle.Witch.DiscardPile.Count}";*/
    }
}
