using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDropArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var battleCard = eventData.pointerDrag.GetComponent<BattleCard>();
        OnPlayerTarget?.Invoke(battleCard);
    }

    public event Action<BattleCard> OnPlayerTarget;
}
