using UnityEngine;
using UnityEngine.UI;

public class CardBattleDraggable : Draggable
{
    [SerializeField] private GameObject trackerPrefab;

    private FollowMouse followMouse;
    private CanvasGroup canvasGroup;
    private Vector3 position;

    [field: SerializeField] public int CardIndex { get; set; }

    public override GameObject CreateTracker()
    {
        GameObject tracker = Instantiate(trackerPrefab);

        var uiCard = GetComponent<UICardCreation>();
        var trackerCard = tracker.GetComponent<UICardCreation>();

        trackerCard.Create(uiCard.CurrentCard);

        Image[] images = tracker.GetComponentsInChildren<Image>();
        foreach (Image i in images)
        {
            i.raycastTarget = false;
        }

        return tracker;
    }

    public void ReturnToHand()
    {
        transform.position = position;
    }

    protected override void Start()
    {
        base.Start();

        followMouse = GetComponent<FollowMouse>();
        canvasGroup = GetComponent<CanvasGroup>();

        OnDragStart += HandleDragStart;
        OnDragEnd += HandleDragEnd;
    }

    private void HandleDragStart(Draggable draggable)
    {
        position = transform.position;
        followMouse.enabled = true;
        canvasGroup.blocksRaycasts = false;
    }

    private void HandleDragEnd(Draggable draggable)
    {
        followMouse.enabled = false;
        canvasGroup.blocksRaycasts = true;
    }
}
