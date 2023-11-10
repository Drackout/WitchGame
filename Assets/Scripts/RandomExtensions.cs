using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public static class RandomExtensions
{
    public static IList<T> Shuffle<T>(this Random random, IList<T> list)
    {
        int nElements = list.Count;  
        while (nElements > 1) {  
            nElements--;  
            int rng = random.Next(nElements + 1);  
            T value = list[rng];  
            list[rng] = list[nElements];  
            list[nElements] = value;  
        }
        return list;
    }

    public static void writeScrollView(TextMeshProUGUI _svContent, ScrollRect _svScrollRect, string texto)
    {
        if (_svContent.text == "")
            _svContent.text += texto;
        else
            _svContent.text += "\n"+texto;
        // Force scroll down
        _svScrollRect.verticalNormalizedPosition = 0 ;
    }

   
}
