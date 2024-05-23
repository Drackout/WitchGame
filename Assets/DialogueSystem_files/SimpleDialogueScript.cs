using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace DialogueSystem
{
    public class SimpleDialogueScript : IDialogueScript
    {
        // List of NodeDatas and their respective keys
        // Works in the same way as a dictionary
        [SerializeField]
        private List<NodeData> dialogueNodes =
            new List<NodeData>();

        /// <summary>
        /// Constructor for the SimpleDialogueScript class
        /// </summary>
        /// <param name="initialLine">First line of dialogue</param>
        public SimpleDialogueScript(string initialLine)
        {
            //Createa NodeData for the first line
            NodeData initialNode = 
                new NodeData("0", initialLine, default, default, null, null);

            dialogueNodes.Add(initialNode);
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
            if ((current.OutPorts?.Count ?? 0) > 0)
                return this[current.OutPorts?[choice].ID];
            return null;
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
    }
}