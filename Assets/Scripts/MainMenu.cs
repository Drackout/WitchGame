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
    [SerializeField] private SeenDialogue SeenDiag;
    [SerializeField] private DialogCreation[] MainMenuDialogList;

    //Audio
    [SerializeField] private AudioClip selectSoundClip;

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
        BattleSettings settings = BattleSettings.Instance;
        //Debug.Log("What stage?: " + settings.getStageIndex());
        //Debug.Log("was it seen?: " + SeenDiag.seenDialogues[settings.getStageIndex()].ToString());

        // If seen skips automatically
        if (SeenDiag.seenDialogues[settings.getStageIndex()])
        {
            SceneManager.LoadScene("RequestsScreen");
        }
        else
        {
            if (MainMenuDialogList[settings.getStageIndex()] != null)
            {
                DialogueLoader.DialogueToLoad = MainMenuDialogList[settings.getStageIndex()];
                SeenDiag.seenDialogues[settings.getStageIndex()] = true;
                SceneManager.LoadScene("DialogueMK");
            }
            else
            {
                SceneManager.LoadScene("RequestsScreen");
            }
        }
    }

    private void LoadBestiaryScene()
    {
        SoundFXManager.instance.PlaySoundFXClip(selectSoundClip, transform, 1f);
        SceneManager.LoadScene("Bestiary");
    }
    
    private void LoadShopScene()
    {
        SoundFXManager.instance.PlaySoundFXClip(selectSoundClip, transform, 1f);
        SceneManager.LoadScene("Shop");
    }

    private void LoadCrafting()
    {
        SoundFXManager.instance.PlaySoundFXClip(selectSoundClip, transform, 1f);
        SceneManager.LoadScene("DeckManager");
    }

    private void Quit()
    {
        SoundFXManager.instance.PlaySoundFXClip(selectSoundClip, transform, 1f);
        Application.Quit();
    }
}
