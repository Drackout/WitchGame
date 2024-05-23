using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;


namespace DialogueSystem.Editor
{
    /// <summary>
    /// Class responsible for managing the editor window to display
    /// the Node based Dialogue System
    /// </summary>
    public class DialogueControllerGraph : EditorWindow
    {
        /// <summary>
        /// GraphView component of this window
        /// </summary>
        private DialogueControllerGraphView graphview;

        /// <summary>
        /// Method called when this script is enabled
        /// </summary>
        private void OnEnable()
        {            
            CreateGraphView();
        }

        /// <summary>
        /// Method called when this script is disabled
        /// </summary>
        private void OnDisable()
        {
            rootVisualElement.Remove(graphview);
        }


        /// <summary>
        /// Method responsible for opening the Dialogue Window 
        /// where the node system is placed
        /// </summary>
        [MenuItem("Dialogue/Dialogue Controller")]
        public static void OpenDialogueGraphWindow()
        {
            DialogueControllerGraph window = GetWindow<DialogueControllerGraph>();
            window.titleContent = new GUIContent(text: "Dialogue Controller");

        }
    
        /// <summary>
        /// Method responsible for opening the Dialogue Window 
        /// where the node system is placed.
        /// This overload is called when the program tries to load 
        /// an already existing Dialogue
        /// </summary>
        /// <param name="ds">Dialogue to be loaded</param>
        public static void OpenDialogueGraphWindow(DialogueScript ds)
        {
            DialogueControllerGraph window = GetWindow<DialogueControllerGraph>();
            window.Close();
            window = GetWindow<DialogueControllerGraph>();

            window.titleContent = new GUIContent(text: ds.DialogueName);
                           
        }     
        /// <summary>
        /// Creates and adds the GraphView component of this window 
        /// </summary>
        private void CreateGraphView()
        {
            graphview = new DialogueControllerGraphView
            {
                name = "Dialogue Controller"
            };

            graphview.StretchToParentSize();

            rootVisualElement.Add(graphview);
            
        }

       
    }
}
