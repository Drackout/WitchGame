using UnityEditor;

namespace DialogueSystem.Editor
{
    [CustomEditor(typeof(DialogueEdgeInspector))]
    public class DialogueEdgeInspectorEditor : UnityEditor.Editor
    {
        private DialogueEdgeInspector _inspector;
        private SerializedProperty _choiceData, _hideIds;


        public void OnEnable()
        {
            _inspector = target as DialogueEdgeInspector;
            _choiceData = serializedObject.FindProperty("_choiceData");
            _hideIds = _choiceData.FindPropertyRelative("_hideIDs");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _inspector._choiceData.IsHidden = EditorGUILayout.Toggle("IsHidden",_inspector._choiceData.IsHidden);
            EditorGUILayout.LabelField("ID",_inspector._choiceData.ID);

            EditorGUILayout.PropertyField(_hideIds);

            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(target);
        }
    }
}
