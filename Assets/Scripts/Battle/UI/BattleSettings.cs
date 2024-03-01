using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSettings : MonoBehaviour
{
    [SerializeField] private RequestStage[] stages;

    private static BattleSettings instance;
    private RequestData currentRequest;
    private int stageIndex;

    public static BattleSettings Instance => instance;
    public bool ShuffleDeck { get; set; }

    public RequestData CurrentRequest => currentRequest;

    public void NextStage()
    {
        stageIndex = Math.Min(stages.Length - 1, stageIndex + 1);
        currentRequest = stages[stageIndex].GetRequest(RequestDifficulty.Easy);
    }

    public void ChooseRequest(RequestDifficulty difficulty)
    {
        currentRequest = stages[stageIndex].GetRequest(difficulty);
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        ShuffleDeck = true;
        stageIndex = 0;
        currentRequest = stages[0].GetRequest(RequestDifficulty.Easy);
    }
}
