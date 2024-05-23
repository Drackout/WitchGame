using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

[RequireComponent(typeof(DialogueDisplayHandler))]
public class DialogueInputComponent : MonoBehaviour
{
    private DialogueDisplayHandler _ddHandler;

    private void Awake()
    {
        _ddHandler = GetComponent<DialogueDisplayHandler>();
    }


    // void OnMove(InputValue value)
    // {
    //     _ddHandler.Move(value.Get<Vector2>());
    // }

    void OnInteract()
    {
        _ddHandler.Interact();
    }
}
