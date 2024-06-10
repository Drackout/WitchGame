using UnityEngine;

[System.Serializable]
public class DialogueNameText
{    
    [field:SerializeField]
    public CharacterList CharName;

    [field:SerializeField]
    public CharacterEmotions CharEmotion;
    
    [field:SerializeField]
    public EmoteList Emote;

    [field:SerializeField]
    public BloonList Bloon;
    
    [field: SerializeField]
    public string Dialogue;
}