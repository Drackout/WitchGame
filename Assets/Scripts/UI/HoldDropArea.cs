using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldDropArea : DropArea
{
    protected override void Dropped(GameObject obj)
    {
        var draggable = obj.GetComponent<CardBattleDraggable>();
        if (draggable != null)
        {
            OnCardDrop?.Invoke(draggable.CardIndex);
        }
    }

    public event Action<int> OnCardDrop;
}
