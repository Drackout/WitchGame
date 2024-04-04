using UnityEngine;
using UnityEngine.UI;

public class CardDeckDraggable : Draggable
{
    [SerializeField] private GameObject trackerPrefab;

    public override GameObject CreateTracker()
    {
        GameObject tracker = Instantiate(trackerPrefab);

        var uiCard = tracker.GetComponent<UICardCreation>();

        OwnedCard card = GetComponent<OwnedCard>();
        uiCard.Create(PlayerResources.Instance.OwnedCards[card.Index]);

        Image[] images = tracker.GetComponentsInChildren<Image>();
        foreach (Image i in images)
        {
            i.raycastTarget = false;
        }

        return tracker;
    }
}
