using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueLoader : MonoBehaviour
{
    // Get the ScriptableObject static
    public static DialogCreation DialogueToLoad;

    // Fields to fill
    [SerializeField] Image background;
    [SerializeField] Image imgChar1;
    [SerializeField] Image imgChar2;
    [SerializeField] TextMeshProUGUI txtChar1;
    [SerializeField] TextMeshProUGUI txtChar2;
    
    [SerializeField] GameObject charName1;
    [SerializeField] GameObject charName2;
    
    // Gotta be smart
    [SerializeField] TextMeshProUGUI Dialogues;

    int DialoguePage;
    
    void Start()
    {
        UpdateBaseInformation();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left Click");
            UpdateDialogue(true);
        }
        else if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right Click");
            UpdateDialogue(false);
        }
    }
    

    
    private void UpdateBaseInformation()
    {
        // Initial information Load
        background.sprite = DialogueToLoad.Background;
        imgChar1.sprite = DialogueToLoad.Char1.img_Normal;
        imgChar2.sprite = DialogueToLoad.Char2.img_None;
        txtChar1.text = DialogueToLoad.Char1.Name;
        txtChar2.text = DialogueToLoad.Char2.Name;
        
        DialoguePage = -1;

        UpdateDialogue(true);
    }

    private void UpdateDialogue(bool changePage)
    {
        if (DialogueToLoad.AllDialogues.Count > 0)
        {
        
            if (changePage == false && DialoguePage != 0)
                DialoguePage--;

            if (DialoguePage == DialogueToLoad.AllDialogues.Count-1)
                EndDialogue();
            else if (changePage == true && !(DialoguePage > DialogueToLoad.AllDialogues.Count-1))
                DialoguePage++;

            string charName = DialogueToLoad.AllDialogues[DialoguePage].CharName.ToString();
            Dialogues.text = DialogueToLoad.AllDialogues[DialoguePage].Dialogue;
            
            // If the name in the list is the same as one of the objects, use that object 
            // Yes it sux if things aren't as == as should be
            Debug.Log("charDiag: " + charName + ", txtChar1: " + txtChar1.text);
            Debug.Log("charDiag: " + charName + ", txtChar2: " + txtChar2.text);

            if (charName != "Moira")
            {
                txtChar2.text = charName;
            }

            // 1st is change, 2nd to put normal (gets weird without)
            if (charName == txtChar1.text)
            {
                ChangeCharExpression(DialogueToLoad.Char1, DialogueToLoad.Char2, imgChar1, imgChar2);
                charName1.SetActive(true);
                charName2.SetActive(false);
            }
            else if (charName == txtChar2.text)
            {
                ChangeCharExpression(DialogueToLoad.Char2, DialogueToLoad.Char1, imgChar2, imgChar1);
                charName1.SetActive(false);
                charName2.SetActive(true);
            }
            else
                Debug.Log("Check if names equal in CharacterList and ScriptableObject");
        }
        else
        {
            Dialogues.text = "No Dialogue loaded!";
        }
        
    }

    private void ChangeCharExpression(CharacterCreator CharacterEmote, CharacterCreator CharacterNormal, Image CharToEmote, Image CharToNormal)
    {
        switch (DialogueToLoad.AllDialogues[DialoguePage].CharEmotion)
        {
            case CharacterEmotions.Normal:
                CharToEmote.sprite = CharacterEmote.img_Normal; break;
            case CharacterEmotions.Happy:
                CharToEmote.sprite = CharacterEmote.img_Happy; break;
            case CharacterEmotions.Sad:
                CharToEmote.sprite = CharacterEmote.img_Sad; break;
            case CharacterEmotions.Angry:
                CharToEmote.sprite = CharacterEmote.img_Angry; break;
            case CharacterEmotions.None:
                CharToEmote.sprite = CharacterEmote.img_None; break;
            default:
                CharToEmote.sprite = CharacterEmote.img_Normal; break;
        }
        // If it needs the char not talking to reset to normal
        //CharToNormal.sprite = CharacterNormal.img_Normal; 
    }

    public void EndDialogue()
    {
        SceneManager.LoadScene(DialogueToLoad.SceneToGo);
    }

}
