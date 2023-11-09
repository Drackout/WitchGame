using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class botaoOnClick : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI    _whereWrite;
    [SerializeField] private ScrollRect         _scrollRect;
    
    public void writeStuff(string texto)
    {
        _whereWrite.text += "\n"+texto;

        // Force scroll down
        _scrollRect.verticalNormalizedPosition = 0 ;
    }
}
