using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShield : MonoBehaviour
{
    [SerializeField] private GameObject[] shieldElements;

    public Shield Shield
    {
        set
        {
            if (shieldElements == null) return;

            for (int i = 0; i < shieldElements.Length; i++)
            {
                if (i < value.Charges)
                {
                    shieldElements[i].SetActive(true);
                    shieldElements[i].GetComponent<Image>().color = value.Element switch
                    {
                        Element.Fire => Color.red,
                        Element.Grass => Color.green,
                        Element.Water => Color.blue,
                        _ => throw new ArgumentException("Unknown element")
                    };
                }
                else
                {
                    shieldElements[i].SetActive(false);
                }
            }
        }
    }
}
