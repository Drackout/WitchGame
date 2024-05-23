using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Class responsible for storing the Data that represents a Dialogue Node
    /// </summary>
    [System.Serializable]
    public class NodeData
    {
        [SerializeField] [HideInInspector]
        private string presetName;

        /// <summary>
        /// Unique id of this Node
        /// </summary>
        [SerializeField]
        private string guid;

        /// <summary>
        /// Dialogue text of this Node
        /// </summary>
        [SerializeField]
        private string dialogue;

        /// <summary>
        /// The position on the GraphView of this Node
        /// </summary>
        [SerializeField] [HideInInspector]
        private Rect position;

        /// <summary>
        /// Variable that defines if the Node is connected to the "Start" node
        /// </summary>
        [SerializeField] [HideInInspector]
        private bool isStart;

        [SerializeField] [HideInInspector]
        private List<RuntimeEventData> events;
        public List<RuntimeEventData> Events {
            get
            {
                return events;
            } 
        }

        /// <summary>
        /// List of Choice data connected to the ouput ports of the Node
        /// </summary>
        [SerializeField] [HideInInspector]
        private List<ChoiceData> outPorts = new List<ChoiceData>();

        /// <summary>
        /// List of functions in the dialogue
        /// </summary>
        [field: SerializeField]
        [field: SerializeReference]
        public List<CustomFunction> CustomFunctions { get; set; }

        /// <summary>
        /// Property that defines the unique id of this Node
        /// </summary>
        public string GUID
        {
            get { return guid; }
            set { guid = value; }
        }
       
        /// <summary>
        /// Property that defines the Dialogue text of this Node
        /// </summary>
        public string Dialogue
        {
            get { return dialogue; }
            set { dialogue = value; }
        }     

        /// <summary>
        /// Property that defines the position of this Node on the GraphView
        /// </summary>
        public Rect Position => position;
        
        /// <summary>
        /// Property that defines if the Node is connected to the "Start" node
        /// </summary>
        public bool IsStart => isStart;

        /// <summary>
        /// Property that defines the list of Choice data connected to
        /// the ouput ports of the Node
        /// </summary>
        public List<ChoiceData> OutPorts => outPorts;

         [field:SerializeField] [HideInInspector]
        public int PresetName { get; private set; }

        [field:SerializeField] [HideInInspector]
        public int ExpressionId { get; private set; }

        /// <summary>
        /// Constructor for this class
        /// </summary>
        /// <param name="guID">Unique id of this node</param>
        /// <param name="dialogue">Dialogue text of this node</param>
        /// <param name="pos">Node position in the graphView</param>
        /// <param name="start">Boolean that defines if the node is connected
        /// to the "Start"</param>
        /// <param name="outPorts">List of Choice data connected to the node</param>
        public NodeData(string guID, string dialogue, Rect pos, bool start, ICollection<ChoiceData> outPorts, 
            List<RuntimeEventData> events, int presetNames = -1, int expressionId = -1)
        {
            isStart = start;
            position = pos;
            GUID = guID;
            Dialogue = dialogue;
            this.outPorts = outPorts as List<ChoiceData>;
            this.events = events;
            PresetName = presetNames;
            ExpressionId = expressionId;    
        }
    }
}















