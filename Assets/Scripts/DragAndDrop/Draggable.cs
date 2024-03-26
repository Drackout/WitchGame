using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private string group;
    [SerializeField] private GameObject trackerPrefab;

    public string Group => group;

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnDragStart?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDragEnd?.Invoke(this);
    }

    private void Start()
    {
        var manager = GetComponentInParent<DragAndDropManager>();
        manager.Register(this, group);
    }

    public event Action<Draggable> OnDragStart;
    public event Action<Draggable> OnDragEnd;
}
