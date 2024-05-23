using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class DialogueContainer : MonoBehaviour
{
    [SerializeField]
    private DialogueScript _script;

    private DialogueDisplayHandler _handler;

    void Start()
    {
        _handler =
            GameObject.FindObjectOfType<DialogueDisplayHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _handler.StartDialolgue(_script);
        }
    }
}
