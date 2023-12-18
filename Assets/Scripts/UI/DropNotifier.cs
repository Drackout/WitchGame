using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropNotifier : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData data)
    {
        Dropped?.Invoke(data);
    }

    public event Action<PointerEventData> Dropped;
}
