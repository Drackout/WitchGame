using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button RequestScreenButton;
    [SerializeField] private Button WitchAgramButton;
    [SerializeField] private Button WiccapediaButton;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text fireStonesText;
    [SerializeField] private TMP_Text waterStonesText;
    [SerializeField] private TMP_Text grassStonesText;
    [SerializeField] private TMP_Text neutralCardsText;
    
    PlayerResources pr = PlayerResources.Instance;

    public void GoToRequests()
    {
        SceneManager.LoadScene("RequestsScreen");
    }    

    public void GoToWitchAgram()
    {
        SceneManager.LoadScene("WitchAgram");
    }
    
    public void GoToWiccapedia()
    {
        SceneManager.LoadScene("Bestiary");
    }

    private void Start()
    {
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        RequestScreenButton.onClick.AddListener(GoToRequests);
        WitchAgramButton.onClick.AddListener(GoToWitchAgram);
        WiccapediaButton.onClick.AddListener(GoToWiccapedia);
        //fireStonesText.text = $"Fire Stones: {pr.GetStones(Element.Fire)}";
        //waterStonesText.text = $"Water Stones: {pr.GetStones(Element.Water)}";
        //grassStonesText.text = $"Grass Stones: {pr.GetStones(Element.Grass)}";
        //neutralCardsText.text = string.Join("\n", pr.NeutralCards);
    }

    private void Update()
    {
        PlayerResources pr = PlayerResources.Instance;
        goldText.text = $"{pr.Gold}";
    }
    
    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
