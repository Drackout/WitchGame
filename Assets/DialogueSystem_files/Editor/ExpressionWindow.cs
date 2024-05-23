using DialogueSystem;
using System;
using UnityEditor;
using UnityEngine;

public class ExpressionWindow : EditorWindow
{
    private static EntityInfo _entityInfo;
    private static int _entityIndex;
    private static EntityData _entityData;
    private static ExpressionPreset _expressionPreset;
    private static ExpressionWindow _window;

    [MenuItem("Window/Expression Window")] // This will create a new menu item under "Window"
    public static void ShowWindow(ExpressionPreset preset, int entityIndex, EntityData _data)
    {
        _expressionPreset = preset;
        _entityIndex = entityIndex;
        _entityData = _data;
        _entityInfo = _data.data[entityIndex];

        // Get existing open window or if none, make a new one:
        _window = (ExpressionWindow)EditorWindow.GetWindow(typeof(ExpressionWindow));
        _window.titleContent = new GUIContent("Expression Preset");

        _window.Show();
    }

    private void OnGUI()
    {
        if(_entityInfo == null)
        {
            EditorGUILayout.HelpBox("Please assign a Entity Preset.", MessageType.Info);
            return;
        }

        if (_expressionPreset == null)
        {
            EditorGUILayout.HelpBox("Please assign an Expression Preset to display.", MessageType.Info);
          
            if (GUILayout.Button("Create new Empty Preset"))
            {
                _expressionPreset = ScriptableObject.CreateInstance<ExpressionPreset>();


                string expressionUD = Guid.NewGuid().ToString();

                string newPresetName = _entityInfo.EntityName == default ? expressionUD : _entityInfo.EntityName;

                AssetDatabase.CreateAsset(_expressionPreset, $"Assets/Scripts/DialogueSystem/ExpressionPreset/NewExpression_{newPresetName}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();

                _entityData.data[_entityIndex].Expressions = _expressionPreset;
            }

            return;
        }

        if (GUILayout.Button("Add New Expression"))
        {
            _expressionPreset.AddNewExpression();
        }

        if (_expressionPreset.Emotions == null)
        {
            return;
        }

        if (_expressionPreset.Emotions.Count <= 0)
        {
            return;
        }

        GUILayout.Label("Expression Preset:", EditorStyles.boldLabel);

        // Display emotions in grid
        DisplayEmotionsGrid();
    }

    private void DisplayEmotionsGrid()
    {
        Undo.RecordObject(_expressionPreset, "Modify Expression Preset");
        
        int width = (int)_window.position.width;
        int height = (int)_window.position.height;
        GUILayout.BeginVertical();

        int columnCount = 4; // Number of emotions per row

        for (int i = 0; i < _expressionPreset.Emotions.Count; i += columnCount)
        {
 
            GUILayout.BeginHorizontal();

            for (int j = 0; j < columnCount && i + j < _expressionPreset.Emotions.Count; j++)
            {
                GUIStyle contentStyle = new GUIStyle();
                contentStyle.fixedWidth = (width / 4);


                Expression emotion = _expressionPreset.Emotions[i + j];

                GUILayout.BeginVertical(contentStyle);
               
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                emotion.Image =
                    (Sprite)EditorGUILayout.ObjectField(obj: emotion.Image,
                    objType: typeof(Sprite), false,
                    GUILayout.Height(100), GUILayout.Width(100));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                emotion.EmotionName = EditorGUILayout.TextField(emotion.EmotionName, GUILayout.ExpandWidth(true));
                // Add more display options for each emotion if needed

                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();


        if (_expressionPreset != null)
        {
            EditorUtility.SetDirty(_expressionPreset);
            EditorUtility.SetDirty(_entityData);
        }
    }

}
