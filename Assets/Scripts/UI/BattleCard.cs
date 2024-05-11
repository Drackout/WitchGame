using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject trackerPrefab;
    [SerializeField] private GameObject trackerParent;

    private Vector3 initialPosition;
    private GameObject activeTracker;
    private Animator animator;

    public Card CurrentCard { get; set; }
    public int Index { get; set; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = transform.position;
        CreateTracker();
        animator.SetBool("Dragged", true);
        OnCardBeginDrag?.Invoke(this);
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
        animator.SetBool("Dragged", false);
        OnCardEndDrag?.Invoke(this);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void CreateTracker()
    {
        activeTracker = Instantiate(trackerPrefab, trackerParent.transform);
        var uiCard = activeTracker.GetComponent<UICardCreation>();
        uiCard.Create(CurrentCard);
    }

    public event Action<BattleCard> OnCardBeginDrag;
    public event Action<BattleCard> OnCardEndDrag;
}
