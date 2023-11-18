using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnLogShowHide : MonoBehaviour
{
    private bool activeyn;
    private Animator Animator;

    void Start()
    {
        activeyn = false;
        Animator = gameObject.GetComponentInChildren<Animator>();
    }

    public void showLog()
    {
        if (activeyn == false)
        {
            Debug.Log("show");
            Animator.SetTrigger("Show");
            activeyn = true;
        }
        else
        {
            Debug.Log("hide");
            Animator.SetTrigger("Hide");
            activeyn = false;
        }
    }
}
