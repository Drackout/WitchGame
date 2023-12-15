using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UICreature : MonoBehaviour
{
    [SerializeField] private Button targetButton;
    [SerializeField] private Image elementImage;
    [SerializeField] private ResourceBar healthBar;
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite grassSprite;
    [SerializeField] private TMP_Text NmbReceived;
    [SerializeField] private TMP_Text reactions;
    [SerializeField] private Image outlineImage;

    private Animator Animator;

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
        Animator = gameObject.GetComponentInChildren<Animator>();
    }

    public void playAnimation(string animString, int extra)
    {
        if (extra != 99)
        {
            if (extra == -1)
                reactions.text = "weak..";
            else if (extra == 1)
                reactions.text = "Strong!";       
                
            animString += extra.ToString();
        }

        Animator.SetTrigger(animString);
    }

    public void setNumbersReceived(int nreceived, Element element)
    {
        if (element.ToString() == "Fire")
            NmbReceived.color=Color.red;
        else if (element.ToString() == "Water")
            NmbReceived.color=Color.cyan;
        else if (element.ToString() == "Grass")
            NmbReceived.color=Color.green;
        else
            NmbReceived.color=Color.white;

        NmbReceived.SetText("-" + nreceived.ToString());
    }

}
