using UnityEngine;
using UnityEngine.EventSystems;

public class DeckPanelDropArea : DropArea
{
    protected override void Dropped(GameObject obj)
    {
        Debug.Log($" -- DeckPanel: dropped: <{obj.name}>");
        var ownedCard = obj.GetComponent<OwnedCard>();
        Debug.Log($" -- DeckPanel: OwnedCard in obj <{ownedCard != null}>");
        if (ownedCard == null)
        {
            return;
        }

        PlayerResources pr = PlayerResources.Instance;

        Card card = pr.OwnedCards[ownedCard.Index];
        Debug.Log($" -- DeckPanel: card element <{card.Element}>");
        if (card.Element == Element.None)
        {
            return;
        }

        bool success = pr.AddCardToDeck(0, card);
        if (success)
        {
            pr.RemoveCardFromOwned(ownedCard.Index);
        }
    }
}
