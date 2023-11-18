using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardAnimation : MonoBehaviour, IPointerEnterHandler
{
    private Animator Animator;

    void Start()
    {
        Animator = gameObject.GetComponentInChildren<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
     {
        Debug.Log("onOver");
        Animator.Play("onOver");
     }

     public void OnPointerExit(PointerEventData eventDatum)
     {
        Debug.Log("Exit");
        Animator.Play("Exit");
     }
     
}
