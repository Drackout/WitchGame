using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSettings : MonoBehaviour
{
    private static BattleSettings instance;

    public static BattleSettings Instance => instance;
    public bool ShuffleDeck { get; set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        ShuffleDeck = true;

        DontDestroyOnLoad(gameObject);
    }
}
