using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ElementalStone : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Element element;
    [SerializeField] private TMP_Text amountText;

    //Audio
    [SerializeField] private AudioClip GrabSoundClip;
    [SerializeField] private AudioClip DisgrabSoundClip;

    public Element Element => element;

    public void UpdateAmount()
    {
        PlayerResources pr = PlayerResources.Instance;
        amountText.text = pr.GetStones(element).ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnStonePickup?.Invoke(element);
        SoundFXManager.instance.PlaySoundFXClip(GrabSoundClip, transform, 1f);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnStoneDrop?.Invoke();
        SoundFXManager.instance.PlaySoundFXClip(DisgrabSoundClip, transform, 1f);
    }

    private void Start()
    {
        UpdateAmount();
    }

    public event Action<Element> OnStonePickup;
    public event Action OnStoneDrop;
}
