using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueLoader : MonoBehaviour
{
    // Get the ScriptableObject
    [SerializeField] DialogCreation DialogueToLoad;

    // Fields to fill
    [SerializeField] Image background;
    [SerializeField] Image imgChar1;
    [SerializeField] Image imgChar2;
    [SerializeField] TextMeshProUGUI txtChar1;
    [SerializeField] TextMeshProUGUI txtChar2;
    
    // Gotta be smart
    [SerializeField] TextMeshProUGUI CharDialogue;
    [SerializeField] TextMeshProUGUI Dialogues;
    
    void Start()
    {
        // Initial information Load
        background.sprite = DialogueToLoad.Background;
        imgChar1.sprite = DialogueToLoad.Char1.img_Normal;
        imgChar2.sprite = DialogueToLoad.Char2.img_Normal;
        txtChar1.text = DialogueToLoad.Char1.Name;
        Debug.Log(DialogueToLoad.Char1.name);
        txtChar2.text = DialogueToLoad.Char2.Name;

        if (DialogueToLoad.AllDialogues.Count > 0)
        {
            CharDialogue.text = DialogueToLoad.AllDialogues[0].CharName.ToString() + ":";
            Dialogues.text = DialogueToLoad.AllDialogues[0].Dialogue;
        }
        else
        {
            Dialogues.text = "No Dialogue loaded!";
        }
    }
}
