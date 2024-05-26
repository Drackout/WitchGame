using System.Collections;
using UnityEngine;
using DialogueSystem;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;

//Class responsible for the handling of the Dialogue Displaystudent council president, woman, serious, stern, smug, blond_hair, uniform, white eyes, white uniform, witch, evil, white cape,whiteeyes, naked, spreading pussy, pussy leaking, pussy, 
public class DialogueDisplayHandler : MonoBehaviour
{
    [SerializeField]
    private Control _control;

    /// <summary>
    /// Current Dialogue script beeing displayed 
    /// </summary>
    [SerializeField]
    private IDialogueScript currentScript = default;

    /// <summary>
    /// Dialogue Controller related to the current script
    /// </summary>
    private DialogueController _controller;

    /// <summary>
    /// Time between each char of the Dialogue
    /// </summary>
    [SerializeField]
    private float displaySpeed = default;

    /// <summary>
    /// Text component responsible for displaying the Dialogue text
    /// </summary>
    [SerializeField]
    [Header("References")]
    private TextMeshProUGUI dialogueDisplayTarget = default;

    /// <summary>
    /// GameObject that defines the container of the ChoiceButtons
    /// </summary>
    [SerializeField]
    private GameObject buttonLayout = default;

    /// <summary>
    /// Prefab of the ChoiceButton
    /// </summary>
    [SerializeField]
    private GameObject buttonPREFAB = default;

    [SerializeField]
    private GameObject border;

    [SerializeField]
    private Image char1;

    [SerializeField]
    private Image char2;


    /// <summary>
    /// Variable that defines the data of one line of dialogue
    /// </summary>
    private NodeData dialogueLine;

    /// <summary>
    /// Text component of the current line of dialogue
    /// </summary>
    private string dialogueText;

    /// <summary>
    /// Auxiliar vairable that represents the time between each char 
    /// in one line of dialogue
    /// </summary>
    private WaitForSeconds effectSpeed;

    /// <summary>
    /// Variable that defines if tha Dialogue has ended
    /// </summary>
    public bool Ended { get; private set; }

    /// <summary>
    /// Variable that defines if the current line of dialogue is 
    /// currenly beeing displayed 
    /// </summary>
    public bool InDialogue { get; private set; }

    public bool DialogueIsPaused { get; set; }

    public System.Action<IDialogueScript> onStartDialogue;
    public System.Action<NodeData> onStartLine;
    public System.Action<IDialogueScript> onEndDialogue;

    private WaitForSeconds endDelay = new WaitForSeconds(0.01f);

    //Choice being selected during the dialogue
    private int _currentChoiceIndex;
    private List<ChoiceSelector> _choices;
    private bool HasChoices => (dialogueLine.OutPorts?.Count ?? -1) > 0;

    private void Awake()
    {
        _choices = new List<ChoiceSelector>();
    }

    public void Test(string test)
    {
        dialogueDisplayTarget.text = test;
    }

    /// <summary>
    /// Method responsibsle for switching to the passed DialogueScript
    /// </summary>
    /// <param name="script">DiSetActive(false);alogue Script to inicialize</param>
    public void StartDialolgue(IDialogueScript script, DialogueController controller = null)
    {
        border.SetActive(true);

        _controller = controller;

        currentScript = script;
        onStartDialogue?.Invoke(currentScript);
        PrepareNewDialogue();
    }

    /// <summary>
    /// Method responsible for seting up the Dialogue
    /// </summary>
    public void PrepareNewDialogue()
    {
        //Display Specificationss
        effectSpeed = new WaitForSeconds(displaySpeed);

        //Initialize First Line
        dialogueLine = currentScript[0];
        dialogueText = currentScript[0].Dialogue;
        StartLine();
    }

    /// <summary>
    /// Method to select a choice not displayed normally
    /// </summary>
    /// <param name="gUID">Access GUID</param>
    /// <param name="choiceType">Reaction to accessing the choice</param>
    public void SpecialChoice(string gUID, SpecialChoice choiceType)
    {
        if (HasChoices)
        {
            int it = -1;
            foreach (ChoiceData choices in dialogueLine.OutPorts)
            { 
                it++;
                Debug.Log("WHY " + choices.ID);
                if (choices.IsHidden)
                {
                  
                    if (!choices.CanUnhide(gUID)) continue;

                    if (choiceType == global::SpecialChoice.Skip)
                    {
                        Debug.Log("IT " + it);
                        dialogueLine =
                             currentScript.GetNextNode(dialogueLine, it);

                        dialogueText = dialogueLine.Dialogue;

                        StartLine();
                    }

                    if (choiceType == global::SpecialChoice.Change)
                    {
                        choices.IsHidden = !choices.IsHidden;
                        InstatiateChoices();
                        
                    }
                }
               
            }
        }
    }

    /// <summary>
    /// Method responsible for selecting and instantiating the respective 
    /// Choice Buttons of the current Dialogue 
    /// </summary>
    private void InstatiateChoices()
    {

        //Delete all instatiated buttons
        foreach (Transform g in buttonLayout.transform)
        {
            Destroy(g.gameObject);
        }

        _choices.Clear();

        //Get the amount of choices in the dialogue
        int choiceNumb = dialogueLine.OutPorts?.Count ?? 0;
        if (choiceNumb == 0) return;


        for (int i = 0; i < choiceNumb; i++)
        {
            if (dialogueLine.OutPorts[i].ChoiceText == "") continue;
            if (dialogueLine.OutPorts[i].IsHidden) continue;

            GameObject temp = Instantiate(buttonPREFAB, transform.position,
                Quaternion.identity, buttonLayout.transform);


            ChoiceSelector cs = temp.GetComponent<ChoiceSelector>();
            cs.ChangeChoiceText(dialogueLine.OutPorts[i].ChoiceText);
            cs.ChoiceNumb = i;
            cs.NextLine = NextLine;
            _choices.Add(cs);
        }

        if (_control == Control.FullKeyboard)
            _choices[0].SetHighlight(true);

        buttonLayout.SetActive(false);

    }

    public void StartLine()

    {
        if (dialogueLine.CustomFunctions != null)
        {
            //Play Custom Events
            foreach (CustomFunction func in dialogueLine.CustomFunctions)
            {
                func.Invoke();
            }
        }

        //Handle button Layout
        InstatiateChoices();

        if (dialogueText.Trim()  == "")
        {
            
            EndDialogue();
            return;
        }

        DisplayLine();
    }

    /// <summary>
    /// Method responsible for deciding initializing next line of the current 
    /// DialogueScript
    /// </summary>
    /// <param name="choice">The selected choice of the current line</param>
    public void NextLine(int choice)
    {
        NodeData previousLine = dialogueLine;


        dialogueLine =
               currentScript.GetNextNode(dialogueLine, choice);


        if (dialogueLine == null)
        {
            EndDialogue();
            return;
        }

        //This is kinda cringe. Need to rework
        if (previousLine.OutPorts[choice].IsHidden)
        {
            EndDialogue();
            return;
        }

        dialogueText = dialogueLine.Dialogue;



        StartLine();
    }

    /// <summary>
    /// Method responsible for ending the current DialogueScript
    /// </summary>
    private void EndDialogue()
    {
        StartCoroutine("EndDialogueDelay");
        border.SetActive(false);
        dialogueDisplayTarget.text = "";
        StopCoroutine("TypeWriterEffect");
    }

    /// <summary>
    /// Method that starts the next line in the Dialogue
    /// </summary>
    private void DisplayLine()
    {
        onStartLine?.Invoke(dialogueLine);
        StopCoroutine("TypeWriterEffect");
        StartCoroutine("TypeWriterEffect");
    }

    /// <summary>
    /// IEnumerator that creates a TypeWriteEffect
    /// </summary>
    /// <returns></returns>
    IEnumerator TypeWriterEffect()
    {

        //Wait a bit so inputs don't overlap
        yield return endDelay;

        InDialogue = true;
        Ended = false;

        dialogueDisplayTarget.text = "";
        int index = 0;

        while (dialogueText.Length > 0)
        {
            yield return effectSpeed;

            //Feio
            index++;
            //Very feio
            if (dialogueLine.Events != null)
            {
                foreach (RuntimeEventData data in dialogueLine.Events)
                {
                    if (data.TriggerIndex == index)
                    {
                        List<System.Type> typeList = new List<System.Type> { };

                        Component[] components = _controller.GetComponents(typeof(Component));

                        foreach (Component component in components)
                        {
                            typeList.Add(component.GetType());
                        }

                        Object selectedObj = _controller.GetComponent(typeList[data.ClassIndex]);

                        var info = typeList[data.ClassIndex].GetMethod(data.MethodName);

                        info.Invoke(selectedObj, data.Params);
                    }
                }
            }

            char nextChar = dialogueText[0];
            dialogueDisplayTarget.text += nextChar;
            dialogueText = dialogueText.Substring(1);
        }
        Ended = true;
        buttonLayout.SetActive(true);
    }

    IEnumerator EndDialogueDelay()
    {
        yield return endDelay;

        //Call all custom events
        onEndDialogue?.Invoke(currentScript);
        InDialogue = false;
    }

    public void Move(Vector2 value)
    {
        if (!InDialogue) return;
        if (_control != Control.FullKeyboard) return;
        if (DialogueIsPaused) return;

        _choices[_currentChoiceIndex].SetHighlight(false);

        Vector2 inputVector = value;
        float moveInput = inputVector.y;


        int listLenght = _choices.Count;

        //Select option
        if (moveInput > 0) // Move to the next index
        {
            _currentChoiceIndex = (_currentChoiceIndex + 1) % listLenght;
        }
        else if (moveInput < 0) // Move to the previous index
        {
            _currentChoiceIndex = (_currentChoiceIndex - 1 + listLenght) % listLenght;
        }


        _choices[_currentChoiceIndex].SetHighlight(true);
    }

    public void Interact()
    {
        if (!InDialogue) return;
        if (DialogueIsPaused) return;

        if (Ended)
        {
            bool clickSkip = true;
            clickSkip =
                (dialogueLine.OutPorts?.Count ?? 0) <= 0;

            if (!clickSkip)
            {
                if (dialogueLine.OutPorts.Any(x => !x.IsHidden))
                    if (dialogueLine.OutPorts[0].ChoiceText != "")
                        return;
            }

            NextLine(0);
        }
        else
        {

            //Invoke all events
            foreach (RuntimeEventData data in dialogueLine.Events)
            {

                List<System.Type> typeList = new List<System.Type> { };

                Component[] components = _controller.GetComponents(typeof(Component));

                foreach (Component component in components)
                {
                    typeList.Add(component.GetType());
                }

                Object selectedObj = _controller.GetComponent(typeList[data.ClassIndex]);

                var info = typeList[data.ClassIndex].GetMethod(data.MethodName);

                info.Invoke(selectedObj, data.Params);

            }


            dialogueDisplayTarget.text += dialogueText;
            dialogueText = "";
            buttonLayout.SetActive(true);
            Ended = true;
            StopCoroutine("TypeWriterEffect");
        }
    }

}
