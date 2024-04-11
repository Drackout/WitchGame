using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyDropArea : DropArea
{
    public int Index { get; set; }

    protected override void Dropped(GameObject obj)
    {
        var draggable = obj.GetComponent<CardBattleDraggable>();
        if (draggable != null)
        {
            OnEnemyTarget?.Invoke(draggable.CardIndex, Index);
        }
    }

    public event Action<int, int> OnEnemyTarget;
}
