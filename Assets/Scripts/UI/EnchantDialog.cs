using System;
using UnityEngine;
using UnityEngine.UI;

public class EnchantDialog : MonoBehaviour
{
    [SerializeField] private UICardCreation sourceCard;
    [SerializeField] private UICardCreation resultCard;
    [SerializeField] private Image stone;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    public Card SourceCard
    {
        set
        {
            sourceCard.Create(value);
        }
    }

    public Card ResultCard
    {
        set
        {
            resultCard.Create(value);
        }
    }

    public Sprite Stone
    {
        set
        {
            stone.sprite = value;
        }
    }

    private void Start()
    {
        confirmButton.onClick.AddListener(() => OnConfirm?.Invoke());
        cancelButton.onClick.AddListener(() => OnCancel?.Invoke());
    }

    public event Action OnConfirm;
    public event Action OnCancel;
}
