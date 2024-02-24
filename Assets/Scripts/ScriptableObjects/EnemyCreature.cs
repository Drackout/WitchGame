using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/CreateEnemy")]
public class EnemyCreature : ScriptableObject
{
    public int health;
    public int attackMin;
    public int attackMax;
    public Element element;
    public string whatHits;
    public string specialEffect;
    public string[] drops;
    public UICreature prefab;
    public GameObject meshPrefab;
}

