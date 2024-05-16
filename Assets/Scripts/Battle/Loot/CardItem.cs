using UnityEngine;

[CreateAssetMenu(fileName="CardItem", menuName="ScriptableObjects/Items/Card")]
public class CardItem : Item
{
    public Element element;
    public CardType type;
    public int power;
}
