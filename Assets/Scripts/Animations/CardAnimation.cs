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
      Debug.Log("pOver");
      Animator.SetTrigger("pOver");
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      Debug.Log("pExit");
      Animator.SetTrigger("pExit");
   }
   

   public void OnPointerClick(PointerEventData eventData)
   {
      Debug.Log("pClick1");
      Animator.SetTrigger("pClick1");
   }

   public void ClickPlay()
   {
      Debug.Log("pClick2");
      Animator.SetTrigger("pClick2");
   }

   public void ClickHold()
   {
      Debug.Log("pHold");
      Animator.SetTrigger("pHold");
   }

   // Set the starting position of every card to default
   public void CardsDefaultPosition()
   {
      Debug.Log("pDefault");
      Animator.SetTrigger("pDefault");
   }
     
}
