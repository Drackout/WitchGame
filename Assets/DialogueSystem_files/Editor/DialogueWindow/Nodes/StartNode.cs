using System;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    public class StartNode : DefaultNode
    {

        //Maybe tou a ser triste e devia usar isto em vez da 
        //variavel starting point 
        DialogueNode startingNode;
        
        public StartNode()
        {

            mainContainer.style.backgroundColor = Color.green;

            title = "Start Node";
            GUID = Guid.NewGuid().ToString();
            
            
            Port port = InstatiateOutputPort();
            outputContainer.Add(port);
            port.portName = "Begin";

            port.portColor = new Color(0.5f,0.7f,0f);

            port.RegisterCallback<MouseUpEvent>((MouseUpEvent evt) =>
            {            
                if (port.connected) 
                {
                    if(startingNode != null)
                        startingNode.EntryPoint = false;

                    foreach (Edge e in port.connections)
                    {
                        startingNode = (e.input.node as DialogueNode);
                        startingNode.EntryPoint = true;
                    }                                     
                }
            });


            RefreshExpandedState();
            RefreshPorts();
        }

    }

}