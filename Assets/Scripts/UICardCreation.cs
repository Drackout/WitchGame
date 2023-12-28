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

    [SerializeField] private Sprite circleWater;
    [SerializeField] private Sprite circleFire;
    [SerializeField] private Sprite circleGrass;

    [SerializeField] private TextMeshProUGUI powerNumber;
    [SerializeField] private Image powerCircle;
    
    [SerializeField] private TextMeshProUGUI tooltipText;

    [SerializeField] private Animator cardAnimator;

    private GameObject activeIcon;
    private int strongDmg;
    private int weakDmg;

    public void Create(Card toShow)
    {
        if (activeIcon != null)
        {
            Destroy(activeIcon);
        }

        GameObject icon = null;
        strongDmg = Convert.ToInt16(powerNumber.text)*2;
        weakDmg = Convert.ToInt16(powerNumber.text)/2;
        // cardAnimator.SetTrigger("pNormal"); // Breaks the animations

        switch (toShow.Type)
        {
            case CardType.Shield:
                if (toShow.Element == Element.Fire)
                {
                    icon = fireShield;
                    tooltipText.text = "Grass Immunity";
                }
                else if (toShow.Element == Element.Grass)
                {
                    icon = grassShield;
                    tooltipText.text = "Water Immunity";
                }
                else if (toShow.Element == Element.Water)
                {
                    icon = waterShield;
                    tooltipText.text = "Fire Immunity";
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
                    tooltipText.text = $"{powerNumber.text} Heal\n {strongDmg} w/ Grass Shield\n {weakDmg} w/ Water Shield ";
                }
                else if (toShow.Element == Element.Grass)
                {
                    icon = grassHeal;
                    tooltipText.text = $"{powerNumber.text} Heal\n {strongDmg} w/ Water Shield\n {weakDmg} w/ Fire Shield ";
                }
                else if (toShow.Element == Element.Water)
                {
                    icon = waterHeal;
                    tooltipText.text = $"{powerNumber.text} Heal\n {strongDmg} w/ Fire Shield\n {weakDmg} w/ Grass Shield ";
                }
                else
                {
                    icon = neutralHeal;
                    tooltipText.text = $"{powerNumber.text} Heal";
                }
                break;

            case CardType.Sword:
                if (toShow.Element == Element.Fire)
                {
                    icon = fireSword;
                    tooltipText.text = $"{powerNumber.text} Fire Damage\n {strongDmg} VS Grass\n {weakDmg} VS Water";
                }
                else if (toShow.Element == Element.Grass)
                {
                    icon = grassSword;
                    tooltipText.text = $"{powerNumber.text} Grass Damage\n {strongDmg} VS Water\n {weakDmg} VS Fire";
                }
                else if (toShow.Element == Element.Water)
                {
                    icon = waterSword;
                    tooltipText.text = $"{powerNumber.text} Water Damage\n {strongDmg} VS Fire\n {weakDmg} VS Grass";
                }
                else
                {
                    icon = neutralSword;
                    tooltipText.text = $"{powerNumber.text} Damage";
                }
                break;

            case CardType.Spell:
                if (toShow.Element == Element.Fire)
                {
                    icon = fireBook;
                    tooltipText.text = $"{powerNumber.text} Fire Damage\n {strongDmg} VS Grass\n {weakDmg} VS Water";
                }
                else if (toShow.Element == Element.Grass)
                {
                    icon = grassBook;
                    tooltipText.text = $"{powerNumber.text} Grass Damage\n {strongDmg} VS Water\n {weakDmg} VS Fire";
                }
                else if (toShow.Element == Element.Water)
                {
                    icon = waterBook;
                    tooltipText.text = $"{powerNumber.text} Water Damage\n {strongDmg} VS Fire\n {weakDmg} VS Grass";
                }
                else
                {
                    icon = neutralBook;
                    tooltipText.text = $"{powerNumber.text} Damage";
                }
                break;
        }


        activeIcon = Instantiate(icon, transform.position, Quaternion.identity, transform);

        powerNumber.text = toShow.Power.ToString();

        if (toShow.Element == Element.Fire)
        {
            //powerNumber.color = colorRed;
            powerCircle.sprite = circleFire;
        }
        if (toShow.Element == Element.Grass)
        {
            //powerNumber.color = colorGreen;      
            powerCircle.sprite = circleGrass;      
        }
        if (toShow.Element == Element.Water)
        {
            //powerNumber.color = colorBlue;            
            powerCircle.sprite = circleWater;
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
