using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Animator anim;

    public void OnPointerClick(PointerEventData pointerEventData)
    {        
        animChangeBool();
    }

    public void animChangeBool()
    {
        if (anim.GetBool("Active") == false)
            anim.SetBool("Active", true);
        else
            anim.SetBool("Active", false);
    }

}
