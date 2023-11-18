using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator Animator;

    void Start()
    {
        Animator = gameObject.GetComponentInChildren<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
     {
        Debug.Log("pOver");
        Animator.SetTrigger("pOver");
     }

     public void OnPointerExit(PointerEventData eventData)
     {
        Debug.Log("pExit");
        Animator.SetTrigger("pExit");
     }

     public void ClickPlay()
     {
        Debug.Log("pClick");
        Animator.SetTrigger("pClick");
     }

     public void ClickHold()
     {
        Debug.Log("pHold");
        Animator.SetTrigger("pHold");
     }
     
}
