using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Class responsible for storing the data of a complete dialogue script
    /// </summary>
    public class DialogueScript : ScriptableObject, IDialogueScript, 
        IEquatable<DialogueScript>
    {
        /// <summary>
        /// Name of the Dialogue
        /// </summary>
        [SerializeField]
        public string DialogueName; 

        /// <summary>
        /// Unique Id of the Dialogue
        /// </summary>
        private int dialogueID;

        /// <summary>
        /// List of NodeDatas and their respective keys
        /// Works in the same way as a dictionary
        /// </summary>
        [SerializeField]
        private List<NodeData> dialogueNodes;

        /// <summary>
        /// Amount of Nodes in the Dialogue
        /// </summary>
        public int Count => dialogueNodes.Count;


        /// <summary>
        /// Method responsible for adding a new NodeData to the
        /// DialogueScript
        /// </summary>
        /// <param name="nd">NodeData to be added</param>
        public void FillDialogueDic(NodeData data)
        {
            if(dialogueNodes == null)
                dialogueNodes = new List<NodeData> { };

            dialogueNodes.Add(data);
        }

        public void Clear()
        {
            dialogueNodes.Clear();
        }


        /// <summary>
        /// Get the data by passing the guid
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public NodeData this[string key]
        {
            get 
            {
                foreach (NodeData data in dialogueNodes)
                {
                    if (data.GUID == key)
                        return data;
                }
                return null;
            }
            private set { }
        }

        /// <summary>
        /// Get the data be passing the index
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public NodeData this[int key]
        {
            get
            {
                return dialogueNodes[key];
            }
            private set { }
        }

        /// <summary>
        /// Method responsible for getting the NodeData that follows 
        /// the passed one
        /// </summary>
        /// <param name="current">the current NodeData 
        /// to iterate upon</param>
        /// <param name="choice">The choice that defines which Node Data
        /// to be returned</param>
        /// <returns>The NodeData that follows the passed one</returns>
        public NodeData GetNextNode(NodeData current, int choice = 0)
        {
            if (current.OutPorts.Count > 0)
                return this[current.OutPorts?[choice].ID];
            return null;
        }


        /// <summary>
        /// Method responsible for iterating the list of IOData
        /// and returning them one by one
        /// </summary>
        /// <returns>The next IOData in line</returns>
        public IEnumerator<NodeData> GetEnumerator()
        {
            foreach (NodeData data in dialogueNodes)
            {
                yield return data;
            }
        }


        /// <summary>
        /// Method responsible for allowing the use of foreach in 
        /// with this class
        /// </summary>
        /// <returns>The next IOData in line</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(DialogueScript other)
        {
            return dialogueID == other.dialogueID;
        }
    }
}


