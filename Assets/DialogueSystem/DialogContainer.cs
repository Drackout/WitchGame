using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class DialogContainer : MonoBehaviour
{
    [SerializeField] private DialogueScript _script;

    private DialogueDisplayHandler _handler;

    void Start()
    {
        _handler = GameObject.FindObjectOfType<DialogueDisplayHandler>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _handler.StartDialolgue(_script);
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    _handler.StartDialolgue(_script);
        //}
    }
}
