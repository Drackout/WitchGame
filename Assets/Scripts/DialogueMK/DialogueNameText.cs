using UnityEngine;

[System.Serializable]
public class DialogueNameText
{    
    [field:SerializeField]
    public CharacterList CharName;

    [field:SerializeField]
    public CharacterEmotions CharEmotion;
    
    [field: SerializeField]
    public string Dialogue;
}