using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
   private Animator Animator;

   void Start()
   {
       Animator = gameObject.GetComponentInChildren<Animator>();
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      //Debug.Log("pOver");
      Animator.ResetTrigger("pExit");
      Animator.SetTrigger("pOver");
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      //Debug.Log("pExit");
      Animator.ResetTrigger("pOver");
      Animator.SetTrigger("pExit");
   }
   

   public void OnPointerClick(PointerEventData eventData)
   {
   }

   public void ClickHold()
   {
      //Debug.Log("pHold");
      Animator.ResetTrigger("pOver");
      Animator.ResetTrigger("pClick1");
      Animator.SetTrigger("pHold");
   }

}
