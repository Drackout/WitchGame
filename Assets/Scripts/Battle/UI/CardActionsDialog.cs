using System;
using UnityEngine;
using UnityEngine.UI;

public class CardActionsDialog : MonoBehaviour
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

    public event Action OnPlay;
    public event Action OnHold;
    public event Action OnClose;
}
