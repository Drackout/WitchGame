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
    [SerializeField] private Button quitButton;

    private void Start()
    {
        shuffleDeckToggle.isOn = BattleSettings.Instance.ShuffleDeck;
        shuffleDeckToggle.onValueChanged.AddListener(ToggleShuffleDeck);

        battleButton.onClick.AddListener(LoadBattleScene);
        
        bestiaryButton.onClick.AddListener(LoadBestiaryScene);

        quitButton.onClick.AddListener(Quit);
    }

    private void ToggleShuffleDeck(bool state)
    {
        BattleSettings.Instance.ShuffleDeck = state;
    }

    private void LoadBattleScene()
    {
        SceneManager.LoadScene("UI Scene");
    }

    private void LoadBestiaryScene()
    {
        SceneManager.LoadScene("Bestiary");
    }

    private void Quit()
    {
        Application.Quit();
    }
}
