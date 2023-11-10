using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public interface BattleLog
{
    public void writeScrollView(TextMeshProUGUI _svContent, ScrollRect _svScrollRect, string texto)
    {
        _svContent.text += "\n"+texto;
        // Force scroll down
        _svScrollRect.verticalNormalizedPosition = 0 ;
    }
}
