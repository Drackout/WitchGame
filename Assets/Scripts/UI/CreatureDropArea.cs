using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatureDropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] private int index;

    public void OnDrop(PointerEventData eventData)
    {
        var battleCard = eventData.pointerDrag.GetComponent<BattleCard>();
        OnCreatureTarget?.Invoke(battleCard, index);
    }

    public event Action<BattleCard, int> OnCreatureTarget;
}
