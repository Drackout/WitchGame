using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text fireStonesText;
    [SerializeField] private TMP_Text waterStonesText;
    [SerializeField] private TMP_Text grassStonesText;

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Start()
    {
        goldText.text = $"Gold: {PlayerResources.Gold}";

        fireStonesText.text = $"Fire stones: {PlayerResources.GetStones(Element.Fire)}";
        waterStonesText.text = $"Water stones: {PlayerResources.GetStones(Element.Water)}";
        grassStonesText.text = $"Grass stones: {PlayerResources.GetStones(Element.Grass)}";
    }
}
