using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSettings : MonoBehaviour
{
    [SerializeField] private RequestStage[] stages;
    [SerializeField] private ElementConfig elementConfig;
    [SerializeField] private LootTable lootTable;

    private static BattleSettings instance;
    private RequestData currentRequest;
    private int stageIndex;

    public static BattleSettings Instance => instance;
    public bool ShuffleDeck { get; set; }
    public IList<RequestStage> Stages => stages;
    public LootTable LootTable => lootTable;

    public ElementConfig ElementConfig => elementConfig;

    public RequestData CurrentRequest => currentRequest;

    public void NextStage()
    {
        stageIndex = Math.Min(stages.Length - 1, stageIndex + 1);
        currentRequest = stages[stageIndex].GetRequest(RequestDifficulty.Easy);

        SaveManager sm = SaveManager.Instance;
        sm.SaveData.stage = stageIndex;
        sm.Save();
    }

    public void ChooseRequest(RequestDifficulty difficulty)
    {
        currentRequest = stages[stageIndex].GetRequest(difficulty);
    }

    public void ForceRequest(RequestData request)
    {
        currentRequest = request;
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

    private void Start()
    {
        SaveManager sm = SaveManager.Instance;
        sm.onSaveClear += LoadFromSave;
        LoadFromSave();
    }
    
    public IList<EnemyCreature> GetAllEnemiesRequests(RequestData currentRequest)
    {
        IList<EnemyCreature> enemies = new List<EnemyCreature>();

        for (int i = 0; i < currentRequest.encounters.Length; i++)
        {
            for (int j = 0; j < currentRequest.encounters[i].enemies.Length; j++)
            {
                enemies.Add(currentRequest.encounters[i].enemies[j]);
            }
        }

        return enemies;
    }

    public EnemyCreature[] GetEnemiesFirstRequest(RequestData currentRequest)
    {
        for (int i = 0; i < currentRequest.encounters[0].enemies.Length; i++)
        {
            Debug.Log("Enemy "+i+": "+ currentRequest.encounters[0].enemies[i].ToString());
        }

        return currentRequest.encounters[0].enemies;
    }

    private void LoadFromSave()
    {
        SaveManager sm = SaveManager.Instance;
        stageIndex = sm.SaveData.stage;
    }
}
