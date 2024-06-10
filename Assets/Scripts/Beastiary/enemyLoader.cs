using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class enemyLoader : MonoBehaviour
{
    [SerializeField] Image mythImage;
    [SerializeField] Image mythElementImage;
    [SerializeField] Image mythStrongImage;
    [SerializeField] Image mythWeakImage;
    [SerializeField] TMP_Text mythName;
    [SerializeField] TMP_Text mythLore;
    [SerializeField] TMP_Text mythElement;
    [SerializeField] TMP_Text mythStrong;
    [SerializeField] TMP_Text mythWeak;
    [SerializeField] int numMyth;
    [SerializeField] Sprite[] mythSprites;
    [SerializeField] Sprite fireElement;
    [SerializeField] Sprite waterElement;
    [SerializeField] Sprite grassElement;

    IList<enemyList> myths = new List<enemyList>
    {
        new enemyList("0", "Homem do Galho", "There are reports of a strange creature that lives in the woods near Pampilhosa da Serra. They call this creature “Homem do Galho” and describe him as a human-like figure, extremely tall and skinny. His limbs look like tree branches and his face is covered in moss. . \n\n Who claims to have seen him says he stands still looking at people for a few seconds before hiding back into the woods, he is always accompanied by a weird growl.",
        Element.Grass, Element.Water, Element.Fire),

        new enemyList("1", "Dragão Volante", "It used to be the common name by Portuguese people to meteors. They thought these were flying dragons, therefore the name. \n\n Nowadays people know what meteors are, but older generations still refer to them as “Dragões Volantes” ", Element.Fire, Element.Grass, Element.Water),

        new enemyList("2", "Adamastor's Tentacle", "A threat to Portuguese sailors during the XV century. This creature was responsible for most of the shipwrecks in “Cabo das Tormentas” or as it is known now Cape of good hope. \n\n Sailors believe to have defeated Adamastor when they traversed the cape, but he is just waiting for his revenge.", Element.Water, Element.Fire, Element.Grass)
    };

    void Start()
    {
        numMyth = 0;
        updateInfo(numMyth);        
    }

    public void advancePage()
    {
        if (numMyth < (myths.Count-1))
        {
            numMyth++;
            updateInfo(numMyth);    
        }
    }

    public void retrocessPage()
    {
        if (numMyth > 0)
        {
            numMyth--;
            updateInfo(numMyth);    
        }
    }



    void updateInfo(int numMyth)
    {
        mythName.text = myths[numMyth].Name;
        mythLore.text = myths[numMyth].Lore;
        mythElement.text = myths[numMyth].Element.ToString();
        mythStrong.text = myths[numMyth].Strong.ToString();
        mythWeak.text = myths[numMyth].Weak.ToString();

        mythImage.sprite = mythSprites[numMyth];
    }

}
