using UnityEngine;

[CreateAssetMenu(fileName="New Request", menuName="ScriptableObjects/Request")]
public class RequestData : ScriptableObject
{
    public EncounterData[] encounters;
    public string battleScene;
    public string location;

    [TextArea(4, 4)]
    public string story;

    public Sprite thumbnail;
}
