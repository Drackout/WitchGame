using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class OwnedCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int Index { get; set; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnOwnedCardDrag?.Invoke(Index);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnOwnedCardDrop?.Invoke();
    }

    public event Action<int> OnOwnedCardDrag;
    public event Action OnOwnedCardDrop;
}
