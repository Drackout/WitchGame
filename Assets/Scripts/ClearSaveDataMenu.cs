using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ClearSaveDataMenu : MonoBehaviour
{
    [SerializeField] private Button promptButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private GameObject[] states;
    [SerializeField] private SeenDialogue MMDialogues;

    private void Start()
    {
        promptButton.onClick.AddListener(HandlePromptClick);
        confirmButton.onClick.AddListener(HandleConfirmClick);
        cancelButton.onClick.AddListener(HandleCancelClick);

        ChangeState(MenuState.Start);
    }

    private void ChangeState(MenuState state)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (state == (MenuState)i)
            {
                states[i].SetActive(true);
            }
            else
            {
                states[i].SetActive(false);
            }
        }
    }

    private void HandlePromptClick()
    {
        ChangeState(MenuState.Prompt);
    }

    private void HandleCancelClick()
    {
        ChangeState(MenuState.Start);
    }

    private void HandleConfirmClick()
    {
        SaveManager.Instance.ClearSave();
        SceneManager.LoadScene("MainMenu");

        // Reset the room dialogues
        for (int i = 0; i < MMDialogues.seenDialogues.Length; i++)
            MMDialogues.seenDialogues[i] = false;
    }

    private enum MenuState
    {
        Start,
        Prompt,
        Success,
    }
}
