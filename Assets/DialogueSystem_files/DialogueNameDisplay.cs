using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DialogueSystem;

/// <summary>
/// Class that handles the NameDisplay Component of the Dialogue Display
/// </summary>
[RequireComponent(typeof(MaskableGraphic))]
public class DialogueNameDisplay : MonoBehaviour
{
    [SerializeField]
    private DialogueDisplayHandler _dHandler;

    [SerializeField]
    private TextMeshProUGUI _textComponent;

    private MaskableGraphic _image;

    private void Awake()
    {
        _image = GetComponent<MaskableGraphic>();
    }

    private void Start()
    {
        // Get List of PresetData     
        EntityData presetdata = Resources.Load<EntityData>("EntityData");

        //Setup the event on Start Line to change/enable the name display
        
        _dHandler.onStartLine += (NodeData data) =>
        {
            if (data.PresetName != 0)
            {
                _image.enabled = true;
                _textComponent.text = presetdata.presetNames[data.PresetName];
            }
            else
            {
                _image.enabled = false;
                _textComponent.text = "";
            }
        };

        _dHandler.onEndDialogue += (e) =>
        {
            _textComponent.text = "";
            _image.enabled = false;
        };
    }

}
