using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnchantHandler : MonoBehaviour, IDropHandler
{
    public int CardIndex { get; set; }

    public void OnDrop(PointerEventData data)
    {
        GameObject dropped = data.pointerDrag;

        var stone = dropped.GetComponent<ElementalStone>();
        if (stone != null)
        {
            OnStonePlaced?.Invoke(stone.Element, CardIndex);
        }
    }

    public event Action<Element, int> OnStonePlaced;
}
