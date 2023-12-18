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
    
    [SerializeField] private Color32 colorBlue;
    [SerializeField] private Color32 colorRed;
    [SerializeField] private Color32 colorGreen;

    [SerializeField] private TextMeshProUGUI powerNumber;

    private GameObject activeIcon;

    public void Create(Card toShow)
    {
        if (activeIcon != null)
        {
            Destroy(activeIcon);
        }

        GameObject icon = null;

        switch (toShow.Type)
        {
            case CardType.Shield:
                if (toShow.Element == Element.Fire)
                {
                    icon = fireShield;
                }
                else if (toShow.Element == Element.Grass)
                {
                    icon = grassShield;
                }
                else if (toShow.Element == Element.Water)
                {
                    icon = waterShield;
                }
                else
                {
                    icon = neutralShield;
                }
                break;

            case CardType.Heal:
                if (toShow.Element == Element.Fire)
                {
                    icon = fireHeal;
                }
                else if (toShow.Element == Element.Grass)
                {
                    icon = grassHeal;
                }
                else if (toShow.Element == Element.Water)
                {
                    icon = waterHeal;
                }
                else
                {
                    icon = neutralHeal;
                }
                break;

            case CardType.Sword:
                if (toShow.Element == Element.Fire)
                {
                    icon = fireSword;
                }
                else if (toShow.Element == Element.Grass)
                {
                    icon = grassSword;
                }
                else if (toShow.Element == Element.Water)
                {
                    icon = waterSword;
                }
                else
                {
                    icon = neutralSword;
                }
                break;

            case CardType.Spell:
                if (toShow.Element == Element.Fire)
                {
                    icon = fireBook;
                }
                else if (toShow.Element == Element.Grass)
                {
                    icon = grassBook;
                }
                else if (toShow.Element == Element.Water)
                {
                    icon = waterBook;
                }
                else
                {
                    icon = neutralBook;
                }
                break;
        }


        activeIcon = Instantiate(icon, transform.position, Quaternion.identity, transform);

        powerNumber.text = toShow.Power.ToString();

        if (toShow.Element == Element.Fire)
            powerNumber.color = colorRed;
        if (toShow.Element == Element.Grass)
            powerNumber.color = colorGreen;
        if (toShow.Element == Element.Water)
            powerNumber.color = colorBlue;

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
