using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

