using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startingPosition;
    private Animator animator;
    private Button button;
    private Image image;

    public int Index { get; set; }

    public void OnBeginDrag(PointerEventData data)
    {
        animator.enabled = false;
        button.enabled = false;
        image.raycastTarget = false;
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
        animator.enabled = true;
        button.enabled = true;
        image.raycastTarget = true;
        transform.position = startingPosition;
        Debug.Log($"Drag end, {data.pointerDrag}");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }
}
