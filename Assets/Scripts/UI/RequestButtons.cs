using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RequestButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler//, IPointerClickHandler
{
   private Animator Animator;

   void Start()
   {
       Animator = gameObject.GetComponentInChildren<Animator>();
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      //Debug.Log("pOver");
      Animator.ResetTrigger("rbHide");
      Animator.SetTrigger("rbShow");
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      Debug.Log("pExit");
      Animator.ResetTrigger("rbShow");
      Animator.SetTrigger("rbHide");
   }
   

   //public void OnPointerClick(PointerEventData eventData)
   //{
   //   Debug.Log("pClick1");
   //   //Animator.ResetTrigger("pOver");
   //   //Animator.SetTrigger("pClick1");
   //}

}
