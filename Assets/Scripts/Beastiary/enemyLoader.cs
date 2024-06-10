using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class enemyLoader : MonoBehaviour
{
    [SerializeField] Image mythImageShadow;
    [SerializeField] Image mythImage;
    [SerializeField] Image mythElementImage;
    [SerializeField] Image mythStrongImage;
    [SerializeField] Image mythWeakImage;
    [SerializeField] TMP_Text mythName;
    [SerializeField] TMP_Text mythLore;
    [SerializeField] int numMyth;
    [SerializeField] Sprite[] mythSprites;
    [SerializeField] Sprite fireElement;
    [SerializeField] Sprite waterElement;
    [SerializeField] Sprite grassElement;
    [SerializeField] Animator anim;
    [SerializeField] Animator animUI;
    [SerializeField] TMP_Text numAndTotal;
 

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
        animUI.SetTrigger("Right");
        if (numMyth < (myths.Count-1))
        {
            numMyth++;
            updateInfo(numMyth);    
        }
        else if(numMyth == (myths.Count-1))
        {
            numMyth = 0;
            updateInfo(numMyth);    
        }
    }

    public void retrocessPage()
    {
        animUI.SetTrigger("Left");
        if (numMyth > 0)
        {
            numMyth--;
            updateInfo(numMyth);    
        }
        else if(numMyth == 0)
        {
            numMyth = (myths.Count-1);
            updateInfo(numMyth);    
        }
    }



    void updateInfo(int numMyth)
    {
        numAndTotal.text = (numMyth+1).ToString() + "/" + myths.Count;
        mythName.text = myths[numMyth].Name;
        mythLore.text = myths[numMyth].Lore;

        mythElementImage.sprite = ChooseElement(myths[numMyth].Element);
        mythStrongImage.sprite = ChooseElement(myths[numMyth].Strong);
        mythWeakImage.sprite = ChooseElement(myths[numMyth].Weak);

        mythImageShadow.sprite = mythSprites[numMyth];
        mythImage.sprite = mythSprites[numMyth];
        anim.SetTrigger(numMyth.ToString());
    }

    private Sprite ChooseElement(Element elem)
    {
        if (elem == Element.Fire)
            return fireElement;
        if (elem == Element.Water)
            return waterElement;
        if (elem == Element.Grass)
            return grassElement;

        return fireElement;
    }

}
