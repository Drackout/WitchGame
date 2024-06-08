using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Dialogue_", menuName="ScriptableObjects/Dialogue/Dialogue")]
public class DialogCreation : Item
{
    public CharacterCreator Char1;
    public CharacterCreator Char2;
    public Sprite Background;
    public List<DialogueNameText> AllDialogues;
    public string SceneToGo;
}
