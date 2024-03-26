using UnityEngine;
using UnityEngine.EventSystems;

public class CardGridDropArea : DropArea
{
    protected override void Dropped(GameObject obj)
    {
        Debug.Log($" -- CardGrid: dropped: <{obj.name}>");
        PlayerResources pr = PlayerResources.Instance;

        var deckCard = obj.GetComponent<DeckCard>();
        if (deckCard == null)
        {
            return;
        }

        pr.AddCardToOwned(deckCard.Card);
        int index = pr.Decks[0].IndexOf(deckCard.Card);
        pr.RemoveCardFromDeck(0, index);
    }
}
