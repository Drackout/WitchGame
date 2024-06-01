using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("Shop");
    }
    
    public void LoadCutscene(DialogCreation hasScene)
    {
        if (hasScene.name != "NONE")
        {
            DialogueLoader.DialogueToLoad = hasScene; 
            SceneManager.LoadScene("DialogueMK");
        }
        else
            LoadMainMenu();
    }
}
