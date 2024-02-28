using UnityEngine;

[CreateAssetMenu(fileName="New Element Config", menuName="ScriptableObjects/Element Config")]
public class ElementConfig : ScriptableObject
{
    [SerializeField] private MatrixInt elementTable;
    [SerializeField] private float damagePositiveMod = 2f;
    [SerializeField] private float damageNegativeMod = 0.5f;
    [SerializeField] private float healPositiveMod = 2f;
    [SerializeField] private float healNegativeMod = 0.5f;

    public MatrixInt ElementTable => elementTable;
    public float DamagePositiveMod => damagePositiveMod;
    public float DamageNegativeMod => damagePositiveMod;
    public float HealPositiveMod => healPositiveMod;
    public float HealNegativeMod => healNegativeMod;
}
