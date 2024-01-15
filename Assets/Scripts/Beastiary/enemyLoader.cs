using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class enemyLoader : MonoBehaviour
{
    [SerializeField] Image mythImage;
    [SerializeField] TMP_Text mythName;
    [SerializeField] TMP_Text mythLore;
    [SerializeField] TMP_Text mythElement;
    [SerializeField] TMP_Text mythStrong;
    [SerializeField] TMP_Text mythWeak;
    [SerializeField] int numMyth;
    [SerializeField] Sprite[] mythSprites;

    IList<enemyList> myths = new List<enemyList>
    {
        new enemyList("0", "Homem Galho", "Há relatos de uma estranha criatura que vive na floresta junto à aldeia do Pessegueiro. Chamam-lhe o “Homem Galho” e descrevem-no como um ser humanoide extremamente alto e magro, cujos membros se assemelham a galhos de árvores, tendo o rosto parcialmente coberto por musgo. \n\n Quem o viu, afirma que ele fica perplexo a olhar para as pessoas durante alguns instantes, antes de mergulhar na floresta, ouvindo-se sempre um ruído estranho.",
        Element.Grass, Element.Water, Element.Fire),

        new enemyList("1", "Dragão Volante", "É o nome que em Portugal se dá a um meteoro com a forma de um dragão voador. \n\n Chama-se também dragão volante ao fogo aceso em nuvens enroscadas que por vezes faiscam e formam a figura de um dragão.", Element.Fire, Element.Grass, Element.Water),

        new enemyList("2", "Adamastor Tentacle", "Ameaçando os Portugueses com naufrágios, perdições de toda a sorte/Que o menor mal de todos seja a morte. \n\nEspera suma vingança de quem o descobriu.", Element.Water, Element.Fire, Element.Grass)
    };


    // Start is called before the first frame update
    void Start()
    {
        numMyth = 0;
        updateInfo(numMyth);        
    }

    // Update is called once per frame
    void Update()
    {
        
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
