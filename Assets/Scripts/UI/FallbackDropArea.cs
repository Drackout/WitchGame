using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FallbackDropArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var battleCard = eventData.pointerDrag.GetComponent<BattleCard>();
        OnCardDrop?.Invoke(battleCard);
    }

    public event Action<BattleCard> OnCardDrop;
}
