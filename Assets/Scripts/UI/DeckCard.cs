using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DeckCard : MonoBehaviour
{
    [SerializeField] private Color fireTint = Color.white;
    [SerializeField] private Color grassTint = Color.white;
    [SerializeField] private Color waterTint = Color.white;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Image background;

    private Card card;

    public int Index { get; set; }

    public Card Card
    {
        get => card;
        set
        {
            card = value;
            cardNameText.text = value.ToString();
            background.color = value.Element switch
            {
                Element.Fire => fireTint,
                Element.Grass => grassTint,
                Element.Water => waterTint,
                _ => throw new ArgumentException("Unknown element!")
            };
        }
    }

    public int Amount
    {
        set
        {
            amountText.text = $"x{value}";
        }
    }
}
