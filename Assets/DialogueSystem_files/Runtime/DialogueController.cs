using UnityEngine;
using DialogueSystem;
using System;
using System.Reflection;

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    private DController _controller;

    private DialogueDisplayHandler _ddHandler;

    public System.Type type;
    public MethodInfo info;

    void Awake()
    {
        _ddHandler = GameObject.FindObjectOfType<DialogueDisplayHandler>();
    }

    public void Play(IDialogueScript descriptionScript = null)
    {
        

        IDialogueScript scriptToPlay 
            = descriptionScript != null ? descriptionScript : _controller.GetDialogue();

        if (!_ddHandler.InDialogue)
        {
            Debug.Log(scriptToPlay);
            _ddHandler.StartDialolgue(scriptToPlay, this);
        }
    }

    public void InvokeTest()
    {
        object script = this.GetComponent(type);

        info.Invoke(script, new object[] { "Funcionou" });
    }

    public void ChangeDefault(int newDefault)
    {
        Debug.Log("WHAT " + newDefault);
        _controller.ChangeDefault(newDefault);
    }
}
