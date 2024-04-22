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
    
    [SerializeField] private Color32 FireEdge1;
    [SerializeField] private Color32 FireEdge2;
    [SerializeField] private Color32 WaterEdge1;
    [SerializeField] private Color32 WaterEdge2;
    [SerializeField] private Color32 GrassEdge1;
    [SerializeField] private Color32 GrassEdge2;

    [SerializeField] private Sprite circleWater;
    [SerializeField] private Sprite circleFire;
    [SerializeField] private Sprite circleGrass;

    [SerializeField] private TextMeshProUGUI powerNumber;
    [SerializeField] private Image powerCircle;

    [SerializeField] private TextMeshProUGUI tooltipText;

    [SerializeField] private Animator cardAnimator;
    [SerializeField] private Transform iconSpace;

    [SerializeField] private Button cancelButton;
    
    [SerializeField] private Sprite[] cardImages;
    [SerializeField] private Image Image;

    private GameObject activeIcon;
    private int cardNumber;

    public Slider SliderDissolver;
    private Material material;


    private void Start()
    {
        Image image = GetComponent<Image>();
        material = new Material(image.material);
        image.material = material;
    }

    private void Update()
    {
        //Debug.Log(SliderDissolver.value);
        material.SetFloat("_Level", SliderDissolver.value);
    }


    public void Create(Card toShow)
    {
        if (cardImages.Length > 0)
        {
            Image.sprite = cardImages[UnityEngine.Random.Range(0, cardImages.Length)];
            
        }
        
        if (activeIcon != null)
            Destroy(activeIcon);

        GameObject icon = null;
        

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

        activeIcon = Instantiate(icon, transform.position, Quaternion.identity, iconSpace);
        powerNumber.text = toShow.Power.ToString();

        if (toShow.Element == Element.Fire)
        {
            powerCircle.sprite = circleFire;
            material.SetColor("_EdgeColour1", FireEdge1); 
            material.SetColor("_EdgeColour2", FireEdge2); 
        }
        if (toShow.Element == Element.Grass)
         {
            powerCircle.sprite = circleGrass;   
            material.SetColor("_EdgeColour1", GrassEdge1); 
            material.SetColor("_EdgeColour2", GrassEdge2); 
         }   
        if (toShow.Element == Element.Water)
        {
            powerCircle.sprite = circleWater;
            material.SetColor("_EdgeColour1", WaterEdge1); 
            material.SetColor("_EdgeColour2", WaterEdge2); 
        }

        cardNumber = Convert.ToInt16(powerNumber.text);
        

        switch (toShow.Type)
        {
            case CardType.Heal:
                if (toShow.Element == Element.Fire)
                    tooltipText.text = $"{cardNumber} Heal\n {cardNumber*2} w/ Grass Shield\n {cardNumber/2} w/ Water Shield ";
                else if (toShow.Element == Element.Grass)
                    tooltipText.text = $"{cardNumber} Heal\n {cardNumber*2} w/ Water Shield\n {cardNumber/2} w/ Fire Shield ";
                else if (toShow.Element == Element.Water)
                    tooltipText.text = $"{cardNumber} Heal\n {cardNumber*2} w/ Fire Shield\n {cardNumber/2} w/ Grass Shield ";
                else
                    tooltipText.text = $"{cardNumber} Heal";
                break;

            case CardType.Sword:
                if (toShow.Element == Element.Fire)
                    tooltipText.text = $"{cardNumber} Fire Damage\n {cardNumber*2} vs Grass\n {cardNumber/2} vs Water";
                else if (toShow.Element == Element.Grass)
                    tooltipText.text = $"{cardNumber} Grass Damage\n {cardNumber*2} vs Water\n {cardNumber/2} vs Fire";
                else if (toShow.Element == Element.Water)
                    tooltipText.text = $"{cardNumber} Water Damage\n {cardNumber*2} vs Fire\n {cardNumber/2} vs Grass";
                else
                    tooltipText.text = $"{cardNumber} Damage";
                break;

            case CardType.Spell:
                if (toShow.Element == Element.Fire)
                    tooltipText.text = $"{cardNumber} Fire Damage\n {cardNumber*2} vs Grass\n {cardNumber/2} vs Water";
                else if (toShow.Element == Element.Grass)
                    tooltipText.text = $"{cardNumber} Grass Damage\n {cardNumber*2} vs Water\n {cardNumber/2} vs Fire";
                else if (toShow.Element == Element.Water)
                    tooltipText.text = $"{cardNumber} Water Damage\n {cardNumber*2} vs Fire\n {cardNumber/2} vs Grass";
                else
                    tooltipText.text = $"{cardNumber} Damage";
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

        ToggleCancelButton(false);
    }

    public void SetCancelEventListener(UnityAction listener)
    {
        cancelButton.onClick.AddListener(listener);
    }

    public void ToggleCancelButton(bool state)
    {
        cancelButton.gameObject.SetActive(state);
    }
}
