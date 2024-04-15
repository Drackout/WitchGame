using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] private string group;
    [SerializeField] private bool usesTracker = true;

    private GameObject tracker;

    private Animator animator;

    public string Group => group;
    public bool UsesTracker => usesTracker;

    public virtual GameObject CreateTracker()
    {
        GameObject tracker = Instantiate(gameObject);

        tracker.GetComponent<Draggable>().enabled = false;

        // Disable all raycast targets
        Image trackerImage = tracker.GetComponent<Image>();
        if (trackerImage != null)
        {
            tracker.GetComponent<Image>().raycastTarget = false;
        }

        foreach (Image image in tracker.GetComponentsInChildren<Image>())
        {
            image.raycastTarget = false;
        }

        return tracker;
    }

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
        if (animator != null)
        {
            animator.SetTrigger("MouseEnter");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animator != null)
        {
            animator.SetTrigger("MouseExit");
        }
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();

        var manager = GetComponentInParent<DragAndDropManager>();
        manager.Register(this, group);
    }

    public event Action<Draggable> OnDragStart;
    public event Action<Draggable> OnDragEnd;
}
