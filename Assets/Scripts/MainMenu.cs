using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Toggle shuffleDeckToggle;
    [SerializeField] private Button battleButton;
    [SerializeField] private Button bestiaryButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button deckManagerButton;

    private void Start()
    {
        shuffleDeckToggle.isOn = BattleSettings.Instance.ShuffleDeck;
        shuffleDeckToggle.onValueChanged.AddListener(ToggleShuffleDeck);

        battleButton.onClick.AddListener(LoadBattleScene);

        bestiaryButton.onClick.AddListener(LoadBestiaryScene);

        shopButton.onClick.AddListener(LoadShopScene);

        quitButton.onClick.AddListener(Quit);

        deckManagerButton.onClick.AddListener(LoadCrafting);
    }

    private void ToggleShuffleDeck(bool state)
    {
        BattleSettings.Instance.ShuffleDeck = state;
    }

    private void LoadBattleScene()
    {
        SceneManager.LoadScene("RequestsScreen");
    }

    private void LoadBestiaryScene()
    {
        SceneManager.LoadScene("Bestiary");
    }
    
    private void LoadShopScene()
    {
        SceneManager.LoadScene("Shop");
    }

    private void LoadCrafting()
    {
        SceneManager.LoadScene("DeckManager");
    }

    private void Quit()
    {
        Application.Quit();
    }
}
