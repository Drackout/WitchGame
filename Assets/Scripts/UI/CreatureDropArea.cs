using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatureDropArea : MonoBehaviour, IDropHandler
{
    public int Index { get; set; }

    public void OnDrop(PointerEventData eventData)
    {
        var battleCard = eventData.pointerDrag.GetComponent<BattleCard>();
        OnCreatureTarget?.Invoke(battleCard, Index);
    }

    public event Action<BattleCard, int> OnCreatureTarget;
}
