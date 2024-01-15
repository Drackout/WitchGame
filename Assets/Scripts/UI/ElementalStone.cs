using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ElementalStone : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Element element;
    [SerializeField] private TMP_Text amountText;

    public Element Element => element;

    public void UpdateAmount()
    {
        PlayerResources pr = PlayerResources.Instance;
        amountText.text = pr.GetStones(element).ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnStonePickup?.Invoke(element);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnStoneDrop?.Invoke();
    }

    private void Start()
    {
        UpdateAmount();
    }

    public event Action<Element> OnStonePickup;
    public event Action OnStoneDrop;
}
