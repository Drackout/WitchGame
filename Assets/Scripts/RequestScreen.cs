using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RequestScreen : MonoBehaviour
{
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private SeenDialogue GetDifficulty;

    private void Start()
    {
        easyButton.onClick.AddListener(() => LoadRequest(RequestDifficulty.Easy));
        mediumButton.onClick.AddListener(() => LoadRequest(RequestDifficulty.Medium));
        hardButton.onClick.AddListener(() => LoadRequest(RequestDifficulty.Hard));
    }

    private void LoadRequest(RequestDifficulty difficulty)
    {
        BattleSettings settings = BattleSettings.Instance;

        Debug.Log("What stage?: " + settings.getStageIndex());
        Debug.Log("difficulty?: " + difficulty.ToString());

        GetDifficulty.difficultyControl = difficulty.ToString();

        settings.ChooseRequest(difficulty);

        RequestData request = settings.CurrentRequest;
        
        SceneManager.LoadScene(request.battleScene);
    }

}
