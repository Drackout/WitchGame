using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleLog : MonoBehaviour, ILogger
{
    [SerializeField] private TextMeshProUGUI _svContent;
    [SerializeField] private ScrollRect _svScrollRect;

    public void Log(string text)
    {
        if (_svContent.text == "")
            _svContent.text += text;
        else
            _svContent.text += "\n"+text;
        // Force scroll down
        //_svScrollRect.verticalNormalizedPosition = 0 ;
        StartCoroutine(ScrollDown());
    }

    private IEnumerator ScrollDown()
    {
        yield return null;
        _svScrollRect.verticalNormalizedPosition = 0;
    }
}
