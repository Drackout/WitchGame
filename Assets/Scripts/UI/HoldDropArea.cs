using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldDropArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var battleCard = eventData.pointerDrag.GetComponent<BattleCard>();
        OnCardHold?.Invoke(battleCard);
    }

    public event Action<BattleCard> OnCardHold;
}
