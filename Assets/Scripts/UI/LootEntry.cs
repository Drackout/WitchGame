using UnityEngine;
using TMPro;

public class LootEntry : MonoBehaviour
{
    [SerializeField] private Transform prefabSlot;
    [SerializeField] private TMP_Text amountText;

    public GameObject LootObject { get; set; }
    public int Amount { get; set; }

    private void Start()
    {
        LootObject.transform.SetParent(prefabSlot, false);
        amountText.text = $"{Amount}";
    }
}
