using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICreature : MonoBehaviour
{
    [SerializeField] private Button targetButton;
    [SerializeField] private Image elementImage;
    [SerializeField] private ResourceBar healthBar;
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite grassSprite;

    public Button TargetButton => targetButton;

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

    public void SetHealth(int currentHealth, int maxHealth)
    {
        healthBar.Set(currentHealth, maxHealth);
    }

    private void Awake()
    {
        targetButton.interactable = false;
    }
}
