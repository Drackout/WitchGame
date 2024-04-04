using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RequestButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   [SerializeField] RequestDifficulty requestDifficulty;
   [SerializeField] Image[] enemySlots;
   [SerializeField] Image[] elementSlots;
   [SerializeField] Sprite fireElement;
   [SerializeField] Sprite waterElement;
   [SerializeField] Sprite grassElement;
   private Animator Animator;

   void Start()
   {
       Animator = gameObject.GetComponentInChildren<Animator>();
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      //Debug.Log("pOver");

      BattleSettings settings = BattleSettings.Instance;
      settings.ChooseRequest(requestDifficulty);

      EnemyCreature[] enemies = settings.GetEnemiesFirstRequest(settings.CurrentRequest);

      for (int i = 0; i < enemies.Length; i++)
      {
         enemySlots[i].sprite = enemies[i].prefab.TargetButton.image.sprite;

         //Change element shown
         if (enemies[i].element.ToString() == "Fire")
            elementSlots[i].sprite = fireElement;
         else if (enemies[i].element.ToString() == "Water")
            elementSlots[i].sprite = waterElement;
         else
            elementSlots[i].sprite = grassElement;
      }
      
      // Img w/ no enemy = alpha 0
      if (enemies.Length == 1)
      {
         var tempColor = enemySlots[1].color;
         tempColor.a = 0f;
         enemySlots[1].color = tempColor;
         enemySlots[2].color = tempColor;
         elementSlots[1].color = tempColor;
         elementSlots[2].color = tempColor;
      }
      else if (enemies.Length == 2)
      {
         var tempColor = enemySlots[2].color;
         tempColor.a = 0f;
         enemySlots[2].color = tempColor;
         elementSlots[2].color = tempColor;
      }


      Animator.ResetTrigger("rbHide");
      Animator.SetTrigger("rbShow");
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      Debug.Log("pExit");
      Animator.ResetTrigger("rbShow");
      Animator.SetTrigger("rbHide");
   }

}
