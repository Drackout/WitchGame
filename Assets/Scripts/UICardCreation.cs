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
    [SerializeField] private object fireBook;
    [SerializeField] private object fireSword;
    [SerializeField] private object fireHeal;
    [SerializeField] private object fireShield;
    [SerializeField] private object waterBook;
    [SerializeField] private object waterSword;
    [SerializeField] private object waterHeal;
    [SerializeField] private object waterShield;
    [SerializeField] private object grassBook;
    [SerializeField] private object grassSword;
    [SerializeField] private object grassHeal;
    [SerializeField] private object grassShield;
    [SerializeField] private object neutralBook;
    [SerializeField] private object neutralSword;
    [SerializeField] private object neutralHeal;
    [SerializeField] private object neutralShield;
    private void Create(Card toShow)
    {
        switch(toShow.Type)
        {
            case CardType.Shield:
                if (toShow.Element == Element.Fire)
                {
                    //Instantiate<>;
                }
                else if (toShow.Element == Element.Grass)
                {

                }
                else if (toShow.Element == Element.Water)
                {

                }
                else
                {

                }
                break;

            case CardType.Heal:
                if (toShow.Element == Element.Fire)
                {

                }
                else if (toShow.Element == Element.Grass)
                {

                }
                else if (toShow.Element == Element.Water)
                {

                }
                else
                {

                }
                break;

            case CardType.Sword:
                if (toShow.Element == Element.Fire)
                {

                }
                else if (toShow.Element == Element.Grass)
                {

                }
                else if (toShow.Element == Element.Water)
                {

                }
                else
                {

                }
                break;

            case CardType.Spell:
                if (toShow.Element == Element.Fire)
                {

                }
                else if (toShow.Element == Element.Grass)
                {

                }
                else if (toShow.Element == Element.Water)
                {

                }
                else
                {

                }
                break;
        }

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
