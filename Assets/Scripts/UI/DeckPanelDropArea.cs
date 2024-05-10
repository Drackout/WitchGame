using UnityEngine;
using UnityEngine.EventSystems;

public class DeckPanelDropArea : DropArea
{
    [SerializeField] private MessageBox warningMessage;

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

        DeckChangeResult result = pr.AddCardToDeck(0, card);
        if (result == DeckChangeResult.Success)
        {
            pr.RemoveCardFromOwned(ownedCard.Index);
        }
        else if (result == DeckChangeResult.TooManyCards)
        {
            string text = string.Format(
                "You can have at most {0} cards in your deck!",
                pr.MaxCardsInDeck);
            warningMessage.Show(text);
        }
        else if (result == DeckChangeResult.TooManyCopies)
        {
            string text = string.Format(
                "You can have at most {0} of {1}!",
                pr.MaxCopies,
                card.ToString());
            warningMessage.Show(text);
        }
    }
}
