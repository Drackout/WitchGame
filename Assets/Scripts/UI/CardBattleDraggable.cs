using UnityEngine;
using UnityEngine.UI;

public class CardBattleDraggable : Draggable
{
    [SerializeField] private GameObject trackerPrefab;

    [field: SerializeField] public int CardIndex { get; set; }

    public override GameObject CreateTracker()
    {
        GameObject tracker = Instantiate(trackerPrefab);

        var uiCard = tracker.GetComponent<UICardCreation>();

        uiCard.Create(PlayerResources.Instance.OwnedCards[CardIndex]);

        Image[] images = tracker.GetComponentsInChildren<Image>();
        foreach (Image i in images)
        {
            i.raycastTarget = false;
        }

        return tracker;
    }
}
