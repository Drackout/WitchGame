using UnityEngine;

[CreateAssetMenu(fileName="CardItem", menuName="ScriptableObjects/Items/Card")]
public class CardItem : Item
{
    public Element element;
    public CardType type;
    public int power;

    public override GameObject GetLootObject()
    {
        GameObject obj = Instantiate(lootPrefab);
        UICardCreation card = obj.GetComponent<UICardCreation>();
        card.Create(new Card(type, element, power));
        return obj;
    }
}
