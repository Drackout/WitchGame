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
    [SerializeField] Image imgBloonChar1;
    [SerializeField] Image imgBloonChar2;
    
    // Gotta be smart
    [SerializeField] TextMeshProUGUI Dialogues;
    [SerializeField] BloonScriptable BloonScriptList;

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
        imgBloonChar1.sprite = BloonScriptList.img_None;
        imgBloonChar2.sprite = BloonScriptList.img_None;
        
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
            BloonList actualBloon = DialogueToLoad.AllDialogues[DialoguePage].BloonEmote;
            
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
                ChangeBloon(actualBloon, imgBloonChar1, imgBloonChar2);
                charName1.SetActive(true);
                charName2.SetActive(false);
            }
            else if (charName == txtChar2.text)
            {
                ChangeCharExpression(DialogueToLoad.Char2, DialogueToLoad.Char1, imgChar2, imgChar1);
                ChangeBloon(actualBloon, imgBloonChar2, imgBloonChar1);
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
            case CharacterEmotions.Intrigued:
                CharToEmote.sprite = CharacterEmote.img_Intrigued; break;
            case CharacterEmotions.Flattered:
                CharToEmote.sprite = CharacterEmote.img_Flattered; break;
            default:
                CharToEmote.sprite = CharacterEmote.img_Normal; break;
        }
        // If it needs the char not talking to reset to normal
        //CharToNormal.sprite = CharacterNormal.img_Normal; 
    }

    private void ChangeBloon(BloonList BList, Image CharToBloon, Image BloonToNormal)
    {
        switch (BList)
        {
            case BloonList.None:
                CharToBloon.sprite = BloonScriptList.img_None; break;
            case BloonList.Angry:
                CharToBloon.sprite = BloonScriptList.img_Angry; break;
            case BloonList.AngryMark:
                CharToBloon.sprite = BloonScriptList.img_AngryMark; break;
            case BloonList.BloonSquare:
                CharToBloon.sprite = BloonScriptList.img_BloonSquare; break;
            case BloonList.BloonRound:
                CharToBloon.sprite = BloonScriptList.img_BloonRound; break;
            case BloonList.BloonThink:
                CharToBloon.sprite = BloonScriptList.img_BloonThink; break;
            case BloonList.BloonSpiky:
                CharToBloon.sprite = BloonScriptList.img_BloonSpiky; break;
            case BloonList.Dot:
                CharToBloon.sprite = BloonScriptList.img_Dot; break;
            case BloonList.Exclamation:
                CharToBloon.sprite = BloonScriptList.img_Exclamation; break;
            case BloonList.Love:
                CharToBloon.sprite = BloonScriptList.img_Love; break;
            case BloonList.Question:
                CharToBloon.sprite = BloonScriptList.img_Question; break;
            default:
                CharToBloon.sprite = BloonScriptList.img_None; break;
        }
        BloonToNormal.sprite = BloonScriptList.img_None;
    }

    public void EndDialogue()
    {
        SceneManager.LoadScene(DialogueToLoad.SceneToGo);
    }

}
