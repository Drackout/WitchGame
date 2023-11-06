using System;
using UnityEngine;
using UnityEngine.UI;

public class UICreature : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Button targetButton;
    [SerializeField] private Image elementImage;
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite grassSprite;

    public Button TargetButton => targetButton;

    public float Health
    {
        set
        {
            healthBar.value = value;
        }
    }

    public Element Element
    {
        set
        {
            elementImage.sprite = value switch
            {
                Element.Fire => fireSprite,
                Element.Water => waterSprite,
                Element.Grass => grassSprite,
                _ => throw new ArgumentException("Invalid element value")
            };
        }
    }

    private void Awake()
    {
        targetButton.interactable = false;
    }
}
