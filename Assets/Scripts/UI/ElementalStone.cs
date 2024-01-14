using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ElementalStone : MonoBehaviour, IDragHandler
{
    [SerializeField] private Element element;
    [SerializeField] private TMP_Text amountText;

    public Element Element => element;

    public void UpdateAmount()
    {
        PlayerResources pr = PlayerResources.Instance;
        amountText.text = pr.GetStones(element).ToString();
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    private void Start()
    {
        UpdateAmount();
    }
}
