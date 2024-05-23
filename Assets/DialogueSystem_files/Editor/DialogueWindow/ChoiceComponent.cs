using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using PublishersFork;
using System;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Struct that represents the Choice Component of a Dialogue Node
    /// </summary>
    public class ChoiceComponent: VisualElement 
    {
        /// <summary>
        /// Choice text
        /// </summary>
        public string Choice { get; }

        /// <summary>
        /// The Port component that represents the choice in the Node
        /// </summary>
        private Port port;

        /// <summary>
        /// Text Field where the choice text is imputed
        /// </summary>
        private TextField inputField;

        /// <summary>
        /// Constructor of this struct
        /// </summary>
        /// <param name="node">Node where this components is placed</param>
        /// <param name="choice">Choice text</param>
        public ChoiceComponent(DialogueNode node, string choice = "")
        {

            // Associates a stylesheet to our root. Thanks to inheritance, all root’s
            // children will have access to it.
            styleSheets.Add(Resources.Load<StyleSheet>("ChoiceComponent_Style"));

            Choice = "";

            Port generateOutPort = node.InstantiatePort
                (Orientation.Horizontal, Direction.Output,
                Port.Capacity.Single, typeof(string));

            generateOutPort.portName = choice;
            
                        //Create and Add a textField to input the dialogue
            TextField textNode = new TextField();

            textNode.RegisterCallback<ChangeEvent<string>>((ChangeEvent<string> evt) =>
            {
                generateOutPort.portName = evt.newValue;
            });
            textNode.value = choice;
            textNode.multiline = true;
            node.outputContainer.Add(textNode);

            port = generateOutPort;
            inputField = textNode;


        }
    }

}