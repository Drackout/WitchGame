using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardActionsDialog : MonoBehaviour//, IPointerExitHandler
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button holdButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        playButton.onClick.AddListener(EmitOnPlay);
        holdButton.onClick.AddListener(EmitOnHold);
        closeButton.onClick.AddListener(EmitOnClose);
    }

    private void EmitOnPlay()
    {
        Debug.Log("Clicked play button");
        OnPlay?.Invoke();
    }

    private void EmitOnHold()
    {
        Debug.Log("Clicked hold button");
        OnHold?.Invoke();
    }

    private void EmitOnClose()
    {
        Debug.Log("Clicked close button");
        OnClose?.Invoke();
    }

    /* Destroy on leave the box - bugs w/ button animation
    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
    */

    public event Action OnPlay;
    public event Action OnHold;
    public event Action OnClose;
}
