using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] private SeenDialogue GetDifficulty;
    [SerializeField] private DialogCreation[] EasyDials;
    [SerializeField] private DialogCreation[] NormalDials;
    [SerializeField] private DialogCreation[] HardDials;
    [SerializeField] private DialogCreation[] CannonDials;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("Shop");
    }
    
    public void LoadCutscene()
    {
        BattleSettings settings = BattleSettings.Instance;

        Debug.Log("What stage?: " + (settings.getStageIndex()));
        Debug.Log("difficulty?: " + GetDifficulty.difficultyControl);

        if (settings.getStageIndex() == 1 || settings.getStageIndex() == 5)
        {
            if (settings.getStageIndex() == 1)
                DialogueLoader.DialogueToLoad = CannonDials[0];
            if (settings.getStageIndex() == 5)
                DialogueLoader.DialogueToLoad = CannonDials[1];
        }
        else if (GetDifficulty.difficultyControl == "Easy")
            DialogueLoader.DialogueToLoad = EasyDials[settings.getStageIndex()-2];
        else if (GetDifficulty.difficultyControl == "Medium")
            DialogueLoader.DialogueToLoad = NormalDials[settings.getStageIndex()-2];
        else if (GetDifficulty.difficultyControl == "Hard")
            DialogueLoader.DialogueToLoad = HardDials[settings.getStageIndex()-2];
        else
            LoadMainMenu();

        SceneManager.LoadScene("DialogueMK");
    }
}
