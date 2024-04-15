using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCard : MonoBehaviour
{
    private UICardCreation uiCard;

    public UICardCreation UICard => uiCard;

    private void Start()
    {
        uiCard = GetComponentInChildren<UICardCreation>();
    }
}
