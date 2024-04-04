using UnityEngine;

public class CardShortDraggable : Draggable
{
    [SerializeField] private GameObject trackerPrefab;

    public override GameObject CreateTracker()
    {
        GameObject tracker = Instantiate(trackerPrefab);

        DeckCard originalDeckCard = GetComponent<DeckCard>();
        DeckCard trackerDeckCard = tracker.GetComponent<DeckCard>();

        trackerDeckCard.Card = originalDeckCard.Card;

        return tracker;
    }
}
