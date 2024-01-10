using System;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    private int[] stones;
    private int gold;

    public static PlayerResources Instance { get; private set; }

    public int Gold
    {
        get => gold;
        set
        {
            gold = Math.Max(0, value);
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        int elementCount = Enum.GetNames(typeof(Element)).Length;
        stones = new int[elementCount];
    }

    public int GetStones(Element element)
    {
        return stones[(int)element];
    }

    public void SetStones(Element element, int value)
    {
        stones[(int)element] = Math.Max(0, value);
    }
}
