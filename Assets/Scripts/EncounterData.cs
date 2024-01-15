using UnityEngine;

[CreateAssetMenu(fileName="New Encounter", menuName="ScriptableObjects/Encounter")]
public class EncounterData : ScriptableObject
{
    public EnemyCreature[] enemies;
}
