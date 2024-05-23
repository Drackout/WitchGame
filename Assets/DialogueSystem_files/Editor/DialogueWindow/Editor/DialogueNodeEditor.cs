using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace DialogueSystem.Editor
{

    [CustomEditor(typeof(DialogueNode), true)]
    public class DialogueNodeEditor : UnityEditor.Editor
    {
        public void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            Debug.Log("WELp");
        }
    }
}
