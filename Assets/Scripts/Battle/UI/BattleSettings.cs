using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSettings : MonoBehaviour
{
    [SerializeField] private EncounterData[] encounters;

    private static BattleSettings instance;
    private int encounterIndex;

    public static BattleSettings Instance => instance;
    public bool ShuffleDeck { get; set; }

    public EncounterData CurrentEncounter => encounters[encounterIndex];

    public EncounterData NextEncounter()
    {
        encounterIndex = Math.Min(encounters.Length - 1, encounterIndex + 1);
        return CurrentEncounter;
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
        encounterIndex = 0;
    }
}
