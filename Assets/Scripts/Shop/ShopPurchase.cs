using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShopPurchase : MonoBehaviour
{
    [SerializeField] private TMP_Text btnPrice;
    [SerializeField] private GameObject imgSold;
    [SerializeField] private CardType type;
    [SerializeField] private Element element;
    [SerializeField] private int power;
    private int totalGold;
    private int cost;

    public void purchase()
    {
        PlayerResources pr = PlayerResources.Instance;
        totalGold = pr.Gold;
        cost = Convert.ToInt32(btnPrice.text);
        
        if (checkPrice(cost))
        {
            pr.Gold = totalGold - cost;
            // ADD CARD to Owned Cards
            imgSold.SetActive(true);
            pr.OwnedCards.Add(new Card(type, element, power));
        }
    }

    private bool checkPrice(int cost)
    {
        if(cost <= totalGold)
        {
            return true;
        }
        return false;
    }

}
