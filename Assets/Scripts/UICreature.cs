using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreature : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Button targetButton;

    public Button TargetButton => targetButton;

    public float Health
    {
        set
        {
            healthBar.value = value;
        }
    }

    private void Awake()
    {
        targetButton.interactable = false;
    }
}
