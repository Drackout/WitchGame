using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Class responsible for handling the default specifications 
    /// of a node 
    /// </summary>
    public abstract class DefaultNode : Node
    {
        /// <summary>
        /// Property that defines the unique GUID of the Node
        /// </summary>
        public string GUID { get; set; }
        
        /// <summary>
        /// Property that defines the list of output ports 
        /// connected to other Nodes
        /// </summary>
        public IList<ChoiceData> OutPorts { get; set; }

        /// <summary>
        /// Constructor for this class
        /// </summary>
        public DefaultNode()
        {
            OutPorts = new List<ChoiceData>{ };
        }

        /// <summary>
        /// Method responsible for creating a new Port component
        /// </summary>
        /// <param name="node">Node of the new Port</param>
        /// <param name="portDirection">IO direction of the new Port</param>
        /// <param name="capacity">Amount of connections that the 
        /// new Port can have</param>
        /// <returns>New Port</returns>
        private Port GeneratePort(Direction portDirection,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            return InstantiatePort(Orientation.Horizontal,portDirection, capacity, typeof(string));
        }

        protected Port InstatiateOutputPort()
        {
            return GeneratePort(Direction.Output, Port.Capacity.Single);
            
        }

        protected Port InstatiateInputPort()
        {
            Port port = GeneratePort(Direction.Input, Port.Capacity.Multi);
            port.SetEnabled(false);
            return port;
        }

        /// <summary>
        /// Method responsible for changing the visibility mode of the node
        /// </summary>
        /// <param name="value"></param>
        public void SwitchVisibility(bool value)
        {
            float dialogueHeight = mainContainer.ElementAt(1).localBound.height;
            float hiddenHeight = dialogueHeight + 50;

            
            style.maxHeight = value ? 9999 : hiddenHeight;

            titleContainer.visible = value;
            titleContainer.style.maxHeight = value ? 9999 : 20;

            mainContainer.ElementAt(2).style.maxHeight = value ? 9999 : 20;
            mainContainer.ElementAt(2).style.minHeight = value ? 0 : 20;

            topContainer.visible = value;

            extensionContainer.visible = value;


            expanded = value;
        }

        /// <summary>
        /// Override of the ToString method
        /// </summary>
        /// <returns>Node in a string format</returns>
        public override string ToString()
        {
            return $"|ID: {GUID}, Choices: {OutPorts.Count}|";
        }

    }
}