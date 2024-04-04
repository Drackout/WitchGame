using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSettings : MonoBehaviour
{
    [SerializeField] private RequestStage[] stages;
    [SerializeField] private ElementConfig elementConfig;

    private static BattleSettings instance;
    private RequestData currentRequest;
    private int stageIndex;

    public static BattleSettings Instance => instance;
    public bool ShuffleDeck { get; set; }

    public ElementConfig ElementConfig => elementConfig;

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
    
    public void GetAllEnemiesRequests(RequestData currentRequest)
    {
        for (int i = 0; i < currentRequest.encounters.Length; i++)
        {
            for (int j = 0; j < currentRequest.encounters[i].enemies.Length; j++)
            {
                Debug.Log("Scene "+ i + ", Enemy "+j+": "+ currentRequest.encounters[i].enemies[j].ToString());
            }
        }
    }

    public EnemyCreature[] GetEnemiesFirstRequest(RequestData currentRequest)
    {
        for (int i = 0; i < currentRequest.encounters[0].enemies.Length; i++)
        {
            Debug.Log("Enemy "+i+": "+ currentRequest.encounters[0].enemies[i].ToString());
        }

        return currentRequest.encounters[0].enemies;
    }
}
