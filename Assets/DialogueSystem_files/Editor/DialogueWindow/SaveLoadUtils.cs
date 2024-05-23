using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;


namespace DialogueSystem.Editor
{
    /// <summary>
    /// Class responsible for Saving and Loading a DialogueScript
    /// </summary>
    public class SaveLoadUtils
    {
        /// <summary>
        /// Method responsible for saving the Dialogue
        /// </summary>
        /// <param name="view">GraphView component that contains the
        /// Dialogue Nodes</param>
        /// <param name="dialogueName">Name of the passed Dialogue</param>
        public DialogueScript SaveDialogues(GraphView view, string dialogueName)
        {

            if (dialogueName == null)
                dialogueName = "InitialName";

            string path =
                EditorUtility.SaveFilePanelInProject("Save Your Dialogue",
                dialogueName + ".asset", "asset",
                "Please select file name to save dialogue to:",
                "Assets/DialogueSystem/Dialogues");

            if (string.IsNullOrEmpty(path)) return null;

            DialogueScript instance = AssetDatabase.LoadAssetAtPath<DialogueScript>(path);
            bool assetExists = instance != null;


            if (!assetExists)
            {
                instance = ScriptableObject.CreateInstance<DialogueScript>();

            }
            else
            {
                instance.Clear();
            }



            List<Node> nodes =
                view.nodes.ToList();

            List<DialogueNode> result = (from n in nodes
                                         where n is DialogueNode
                                         select n as DialogueNode).ToList();

            result = (from n in result
                      orderby !n.EntryPoint
                      select n).ToList();



            foreach (DialogueNode nd in result)
            {
                bool isConnected = false;

                //Check if they have any input connections
                foreach (Port p in nd.inputContainer.Children())
                {
                    if (p.connected)
                    {
                        isConnected = true;
                    }
                }

                if (!isConnected)
                    continue;

                NodeData data = new NodeData(
                    start: nd.EntryPoint,
                    pos: nd.GetPosition(),
                    guID: nd.GUID,
                    dialogue: nd.DialogText,
                    outPorts: nd.OutPorts.Select(item => item.Clone()).ToList(),
                    events: nd.Events,
                    presetNames: nd.PresetName,
                    expressionId: nd.ExpresionID
                    );


                List<CustomFunction> tempLis = new List<CustomFunction>();
                tempLis.AddRange(nd.CustomFunctions);
                data.CustomFunctions = tempLis;

                instance.FillDialogueDic(data);

            }

            string[] dir = path.Split('/');

            instance.DialogueName = dir[dir.Length - 1].Replace(".asset", "");



            if (!assetExists)
            {
                AssetDatabase.CreateAsset(instance, path);
            }
            else
            {

            }

            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(instance);

            return instance;
        }


        /// <summary>
        /// Method responsible for loading the Dialogue into the 
        /// Dialogue Window
        /// </summary>
        /// <param name="view">GraphView component to display the
        /// Dialogue Nodes into</param>
        /// <param name="script">Dialogue script to Load</param>
        public void LoadDialogues(DialogueGraphView view,
            DialogueScript script)
        {
            view.DialogueName = script.DialogueName;

            //view.nodes.ForEach(x => view.Remove(x));

            foreach (NodeData io in script)
            {
                view.CreateDialogueNode(io);
            }

            foreach (NodeData data in script)
            {
                if (data.IsStart) view.ConnectToStart(data);
                view.InstatiateEdges(data);
            }

        }

    }
}
