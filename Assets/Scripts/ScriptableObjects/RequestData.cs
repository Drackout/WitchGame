using UnityEngine;

[CreateAssetMenu(fileName="New Request", menuName="ScriptableObjects/Request")]
public class RequestData : ScriptableObject
{
    public EncounterData[] encounters;
}
