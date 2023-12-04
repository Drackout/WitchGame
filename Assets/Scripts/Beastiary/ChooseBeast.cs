using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseBeast : MonoBehaviour
{
    [SerializeField] Sprite chooserImg;
    [SerializeField] Image BeastImg;

    public void ClickChooseBeast()
    {
        BeastImg.sprite = chooserImg;
    }

}
