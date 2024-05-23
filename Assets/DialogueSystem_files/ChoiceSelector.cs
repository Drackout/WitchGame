using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for managing the behaviours of a Choice Button
/// </summary>
public class ChoiceSelector : MonoBehaviour
{
    /// <summary>
    /// Property that defines the number of the choice in the Node
    /// </summary>
    public int ChoiceNumb { get; set; }

    /// <summary>
    /// Property that defines the text the choice has
    /// </summary>
    public string ChoiceText { get; private set; }

    /// <summary>
    /// Delegate that stores the numder of this choice
    /// </summary>
    /// <param name="i">Number of this choice</param>
    public delegate void ChangeLine(int i);

    /// <summary>
    /// Property that is used when the ChoiceSelector needs to
    /// change to the enxt line
    /// </summary>
    public ChangeLine NextLine { get; set; }


    [SerializeField]
    private TextMeshProUGUI textDisplay = default;

    [SerializeField]
    private Button _button;


    /// <summary>
    /// Method responsible for changing the text conponent of the 
    /// choice object
    /// </summary>
    /// <param name="choiceText"></param>
    public void ChangeChoiceText(string choiceText) 
    {        
        ChoiceText = choiceText;
        textDisplay.text = choiceText;
    }

    /// <summary>
    /// Method triggered when this Choice Selector is selected
    /// </summary>
    public void SelectChoice()
    {
        NextLine(ChoiceNumb);
    }

    public void SetHighlight(bool toggle)
    {
        if(toggle)
            _button.targetGraphic.color = _button.colors.highlightedColor;
        else
            _button.targetGraphic.color = _button.colors.normalColor;
    }
}
