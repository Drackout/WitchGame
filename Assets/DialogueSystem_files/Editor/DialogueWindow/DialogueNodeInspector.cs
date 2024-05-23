using System.Collections.Generic;
using UnityEngine;


namespace DialogueSystem.Editor
{
    public class DialogueNodeInspector : ScriptableObject
    {
        [SerializeField]
        private bool isStart;

        [SerializeField] [HideInInspector]
        private string dialogueText;

        private DialogueNode node;

        [field: SerializeField][HideInInspector]
        public List<RuntimeEventData> Events {get; set; }

        [field: SerializeField][HideInInspector]
        public List<CustomFunction> CustomFunctions { get; set; }

       
        //[SerializeField]
        private EntityInfo entityInfo;
        
        private int presetEntityName;

        private SavingWaitingList saveWaitingList;
        public SavingWaitingList SaveWaitingList { get => saveWaitingList; set => saveWaitingList = value; }

        public DialogueController Controller { get; private set; }

        public void init(DialogueNode dn, DialogueController controller)
        {
            Controller = controller;
            node = dn;
            isStart = dn.EntryPoint;
            dialogueText = dn.DialogText;
            
            Events = node.Events;
            if(Events == null)
                Events = new List<RuntimeEventData> { };

            presetEntityName = dn.PresetName;
            SaveWaitingList = dn.SaveWatingList;

            CustomFunctions = new List<CustomFunction> { };
            CustomFunctions.AddRange(dn?.CustomFunctions);

            EntityData data = Resources.Load<EntityData>("EntityData");

            int entityId = presetEntityName;
            if(presetEntityName != 0)
            {
                entityId -= 1;
            }

            entityInfo = data.data[entityId];
        }

        public void UpdateNode()
        {
            node.CustomFunctions = CustomFunctions;
        }

        public void ChangeDialogue(string newDialogue, bool fromTxt = false)
        {
            if (!fromTxt)
            {
                node.TextField.value = newDialogue;
                node.DialogText = newDialogue;
                //node.entityInfo = 
            }

            dialogueText = newDialogue;
        }
    }
}

