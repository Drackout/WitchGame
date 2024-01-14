using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private CardGrid cardGrid;
    [SerializeField] private ElementalStone[] stones;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        cardGrid.OnStonePlaced += EnchantCard;
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void EnchantCard(Element element, int index)
    {
        PlayerResources pr = PlayerResources.Instance;

        if (pr.GetStones(element) <= 0)
        {
            return;
        }

        Card card = pr.OwnedCards[index];
        pr.OwnedCards[index] = new Card(card.Type, element, card.Power);

        pr.SetStones(element, pr.GetStones(element) - 1);

        cardGrid.UpdateCard(index);
        foreach (ElementalStone s in stones)
        {
            s.UpdateAmount();
        }
    }
}
