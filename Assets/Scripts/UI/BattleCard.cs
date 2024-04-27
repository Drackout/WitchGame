using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject trackerPrefab;

    private Vector3 initialPosition;
    private GameObject activeTracker;

    public Card CurrentCard { get; set; }
    public int Index { get; set; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = transform.position;
        CreateTracker();
        OnCardBeginDrag?.Invoke(CurrentCard);
    }

    public void OnDrag(PointerEventData eventData)
    {
        return;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = initialPosition;
        Destroy(activeTracker);
        activeTracker = null;
        OnCardEndDrag?.Invoke(CurrentCard);
    }

    private void Start()
    {
    }

    private void CreateTracker()
    {
        activeTracker = Instantiate(trackerPrefab, transform.parent);
        var uiCard = activeTracker.GetComponent<UICardCreation>();
        uiCard.Create(CurrentCard);
    }

    public event Action<Card> OnCardBeginDrag;
    public event Action<Card> OnCardEndDrag;
}
