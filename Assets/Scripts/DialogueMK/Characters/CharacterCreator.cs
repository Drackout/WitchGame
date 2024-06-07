using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName="Char_", menuName="ScriptableObjects/Dialogue/Character")]
public class CharacterCreator : Item
{
    public string Name;
    public Sprite img_Normal;
    public Sprite img_Happy;
    public Sprite img_Angry;
    public Sprite img_Sad;
    public Sprite img_None;
    public Sprite img_Flattered;
    public Sprite img_Intrigued;
}
