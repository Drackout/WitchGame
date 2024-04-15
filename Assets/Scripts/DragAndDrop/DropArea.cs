using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] private string group;

    private DragAndDropManager manager;
    private Image image;

    public void Toggle(bool state)
    {
        image.enabled = state;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"DropArea: object dropped; {eventData.pointerDrag.name}");
        Dropped(eventData.pointerDrag);

        OnDropItem?.Invoke(eventData.pointerDrag);
    }

    protected virtual void Dropped(GameObject obj)
    {}

    private void Start()
    {
        manager = GetComponentInParent<DragAndDropManager>();
        manager.Register(this, group);

        image = GetComponent<Image>();

        Toggle(false);
    }

    private void OnDestroy()
    {
        manager.Deregister(this, group);
    }

    public event Action<GameObject> OnDropItem;
}
