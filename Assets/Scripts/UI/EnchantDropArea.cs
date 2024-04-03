using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnchantDropArea : DropArea
{
    public int CardIndex { get; set; }

    protected override void Dropped(GameObject obj)
    {
        var stone = obj.GetComponent<ElementalStone>();
        if (stone != null)
        {
            OnStonePlaced?.Invoke(stone.Element, CardIndex);
        }
    }

    public event Action<Element, int> OnStonePlaced;
}
