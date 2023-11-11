using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICreature : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text maxHealthText;
    [SerializeField] private TMP_Text currentHealthText;
    [SerializeField] private Button targetButton;
    [SerializeField] private Image elementImage;
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
        maxHealthText.text = maxHealth.ToString();
        currentHealthText.text = currentHealth.ToString();
        healthBar.value = (float)currentHealth / maxHealth;
    }

    private void Awake()
    {
        targetButton.interactable = false;
    }
}
