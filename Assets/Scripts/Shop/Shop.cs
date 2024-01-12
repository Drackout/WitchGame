using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text fireStonesText;
    [SerializeField] private TMP_Text waterStonesText;
    [SerializeField] private TMP_Text grassStonesText;
    [SerializeField] private TMP_Text neutralCardsText;

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Start()
    {
        PlayerResources pr = PlayerResources.Instance;

        goldText.text = $"Gold: {pr.Gold}";

        fireStonesText.text = $"Fire stones: {pr.GetStones(Element.Fire)}";
        waterStonesText.text = $"Water stones: {pr.GetStones(Element.Water)}";
        grassStonesText.text = $"Grass stones: {pr.GetStones(Element.Grass)}";

        neutralCardsText.text = string.Join("\n", pr.NeutralCards);
    }
}
