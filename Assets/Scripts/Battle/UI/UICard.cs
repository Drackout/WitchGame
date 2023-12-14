using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startingPosition;

    public void OnBeginDrag(PointerEventData data)
    {
        startingPosition = transform.position;
        transform.position = Input.mousePosition;
        Debug.Log("Drag start");
    }

    public void OnDrag(PointerEventData data)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData data)
    {
        transform.position = startingPosition;
        Debug.Log($"Drag end, {data.pointerDrag}");
    }
}
