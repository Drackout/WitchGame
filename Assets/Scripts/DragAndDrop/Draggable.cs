using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] private string group;
    [SerializeField] private GameObject trackerPrefab;

    private Animator animator;

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetTrigger("MouseEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetTrigger("MouseExit");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        var manager = GetComponentInParent<DragAndDropManager>();
        manager.Register(this, group);
    }

    public event Action<Draggable> OnDragStart;
    public event Action<Draggable> OnDragEnd;
}
