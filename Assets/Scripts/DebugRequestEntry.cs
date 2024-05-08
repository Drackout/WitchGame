using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DebugRequestEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Button button;

    public void SetRequest(RequestData request, string label)
    {
        this.label.text = label;
        button.onClick.AddListener(() => Load(request));
    }

    private void Load(RequestData request)
    {
        var battleSettings = BattleSettings.Instance;
        battleSettings.ForceRequest(request);
        SceneManager.LoadScene(request.battleScene);
    }
}
