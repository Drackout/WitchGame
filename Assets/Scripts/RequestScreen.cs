using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RequestScreen : MonoBehaviour
{
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;

    private void Start()
    {
        easyButton.onClick.AddListener(() => LoadRequest(RequestDifficulty.Easy));
        mediumButton.onClick.AddListener(() => LoadRequest(RequestDifficulty.Medium));
        hardButton.onClick.AddListener(() => LoadRequest(RequestDifficulty.Hard));
    }

    private void LoadRequest(RequestDifficulty difficulty)
    {
        BattleSettings.Instance.ChooseRequest(difficulty);
        SceneManager.LoadScene("UI Scene");
    }
}
