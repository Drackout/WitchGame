using UnityEngine;
using UnityEngine.EventSystems;

public class CardGridDropArea : DropArea
{
    [SerializeField] private MessageBox warningMessage;

    protected override void Dropped(GameObject obj)
    {
        Debug.Log($" -- CardGrid: dropped: <{obj.name}>");
        PlayerResources pr = PlayerResources.Instance;

        var deckCard = obj.GetComponent<DeckCard>();
        if (deckCard == null)
        {
            return;
        }

        int index = pr.Decks[0].IndexOf(deckCard.Card);
        DeckChangeResult result = pr.RemoveCardFromDeck(0, index);
        if (result == DeckChangeResult.Success)
        {
            pr.AddCardToOwned(deckCard.Card);
        }
        else if (result == DeckChangeResult.NotEnoughCards)
        {
            string text = string.Format(
                "You must have at least {0} cards in your deck!",
                pr.MinCardsInDeck);
            warningMessage.Show(text);
        }
    }
}
