using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    [CustomEditor(typeof(DialogueScript))]
    public class DialogueScriptEditor : UnityEditor.Editor
    {
        SerializedProperty nodeList;

        public void OnEnable()
        {
            nodeList = serializedObject.FindProperty("dialogueNodes");
        }

        public override VisualElement CreateInspectorGUI()
        {
            DialogueScript script = target as DialogueScript;

            VisualElement root = new VisualElement();

            root.Add(new Label("WAKA WAKA"));

            return base.CreateInspectorGUI();

            //return root;
        }
      
    }
}
