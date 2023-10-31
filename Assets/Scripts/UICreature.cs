using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreature : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    public float Health
    {
        set
        {
            healthBar.value = value;
        }
    }
}
