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
    //[SerializeField] private DialogCreation[] diag2;
    //[SerializeField] private DialogCreation[] diag3;
    //[SerializeField] private DialogCreation[] diag4;
    //[SerializeField] private DialogCreation[] diag5;

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
        
        //SceneManager.LoadScene(ChooseCutscene(settings.getStageIndex(), difficulty.ToString(), request.battleScene));
        SceneManager.LoadScene(request.battleScene);
    }

    // Pls dont hit me
    //private string ChooseCutscene(int RequestNum, string difficulty, string req)
    //{
    //    string scene = req;
    //    if (RequestNum == 0)
    //    {
    //        DialogueLoader.DialogueToLoad = diag1[0];
    //        scene = "DialogueMK";
    //    }
    //    else if (RequestNum == 1 && difficulty == "easy")
    //    {
    //        DialogueLoader.DialogueToLoad = diag2[0];
    //        scene = "DialogueMK";
    //    }
    //    else if (RequestNum == 1 && difficulty == "medium")
    //    {
    //        DialogueLoader.DialogueToLoad = diag2[1];
    //        scene = "DialogueMK";
    //    }
    //    else if (RequestNum == 1 && difficulty == "hard")
    //    {
    //        DialogueLoader.DialogueToLoad = diag2[2];
    //        scene = "DialogueMK";
    //    }
    //    if (scene != req)
    //    {
    //        DialogueLoader.DialogueToLoad.SceneToGo = req;
    //    }
    //    return scene;
    //}
}
