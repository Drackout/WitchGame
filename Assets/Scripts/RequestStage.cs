using UnityEngine;
using System;

[CreateAssetMenu(fileName="New Stage", menuName="ScriptableObjects/Stage")]
public class RequestStage : ScriptableObject
{
    [SerializeField] private RequestData easy;
    [SerializeField] private RequestData medium;
    [SerializeField] private RequestData hard;

    public RequestData GetRequest(RequestDifficulty difficulty)
    {
        return difficulty switch
        {
            RequestDifficulty.Easy => easy,
            RequestDifficulty.Medium => medium,
            RequestDifficulty.Hard => hard,
            _ => throw new ArgumentException("Unknown difficulty value")
        };
    }
}
