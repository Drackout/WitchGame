using UnityEngine;
using System.Collections.Generic;

public class DebugBattleLoader : MonoBehaviour
{
    [SerializeField] private IList<RequestData> requests;
    [SerializeField] private DebugRequestEntry requestEntryPrefab;
    [SerializeField] private GameObject container;

    private bool isOpen;

    private void Start()
    {
        IList<RequestStage> stages = BattleSettings.Instance.Stages;

        requests = new List<RequestData>();

        int i = 1;
        foreach (RequestStage stage in stages)
        {
            CreateEntry(i, RequestDifficulty.Easy,
                stage.GetRequest(RequestDifficulty.Easy));
            CreateEntry(i, RequestDifficulty.Medium,
                stage.GetRequest(RequestDifficulty.Medium));
            CreateEntry(i, RequestDifficulty.Hard,
                stage.GetRequest(RequestDifficulty.Hard));
            
            i += 1;
        }

        Toggle(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Debug Battle Menu"))
        {
            Toggle(!isOpen);
            isOpen = !isOpen;
        }
    }

    private void CreateEntry(int stage, RequestDifficulty difficulty,
        RequestData request)
    {
        string label;

        DebugRequestEntry entry = Instantiate(requestEntryPrefab,
            container.transform);
        label = string.Format("Stage {0} - {1} - {2}",
            stage, difficulty, request.battleScene);
        entry.SetRequest(request, label);
    }

    private void Toggle(bool state)
    {
        var containerRect = container.GetComponent<RectTransform>();
        Vector2 pos = containerRect.anchoredPosition;
        pos.x = state ? 0f : 3000f;
        containerRect.anchoredPosition = pos;
    }
}
