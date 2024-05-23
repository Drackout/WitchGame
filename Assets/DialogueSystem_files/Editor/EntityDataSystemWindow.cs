using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.PackageManager.UI;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Wnidow that display the dialogue entity data used in this project
    /// </summary>
    public class EntityDataSystemWindow : EditorWindow 
    {
        //List of entities
        private EntityData _entityData;
        // The variable to control where the scrollview 
        //'looks' into its child elements.
        private Vector2 _scrollPosition;
        // Variable to store the search keyword
        private string _searchKeyword = "";

        /// <summary>
        /// Method responsible for opening the EntityDataSystem Window 
        /// </summary>
        [MenuItem("Dialogue/Entity Data")]
        public static void OpenEntityDataWindow()
        {
            EntityDataSystemWindow window = GetWindow<EntityDataSystemWindow>();
           
            window.titleContent = new GUIContent(text: "Entity Data");
           
            //Get entity 
            window._entityData = GetEntityData();

            window.minSize = new Vector2(750, window.maxSize.y);
                  
        }

        private void OnGUI()
        {

            //Make sure there's no missing data error
            if(_entityData == null)
            {
                EditorGUILayout.HelpBox("Missing Data. This shouldn't happen. Rah Rhow", MessageType.Error);
                return;
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Add New Preset"))
            {
                _entityData.AddNewPreset();
            }

            GUILayout.Space(10);

            // Region: Search Bar
            #region Search Bar
            GUILayout.BeginHorizontal();
            GUILayout.Label("Search: ");
            _searchKeyword = GUILayout.TextField(_searchKeyword);
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(20);

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);        

            for (int i = 0; i < _entityData.data.Count; i++)
            {
                //Update list to the searched entities
                if (_searchKeyword != "")
                {
                    if (_entityData.data[i].EntityName == "")
                    {
                        continue;
                    }
                    // Skip presets that do not match the search keyword
                    if (!(_entityData?.data[i]?.EntityName?.ToLower()?.Contains(_searchKeyword.ToLower()) ?? true))
                    {
                        continue;
                    }
                }
                
                EntityInfo info = _entityData.data[i];
           
                EditorAddOns.DrawUILine(new Color(0.1f, 0.1f, 0.1f, 1), 1, 0);

                // Create a custom GUIStyle with the desired background color
                GUIStyle customButtonStyle = new GUIStyle(GUI.skin.label);
                customButtonStyle.normal.background = EditorAddOns.MakeTexture(1, 1, new Color(0.4f, 0.4f, 0.4f)); // Set the background color here
                customButtonStyle.margin = new RectOffset(0,0,0,0);

                string presetName = (info?.EntityName?.Trim() ?? "") == "" ? $"Preset {i}" : info.EntityName;

                if (GUILayout.Button(presetName, customButtonStyle))
                {
                    _entityData.data[i].Hidden = !_entityData.data[i].Hidden;
                }

                // Hidable Content
                if (_entityData.data[i].Hidden)
                {
                    DrawEntityDataSlot(info, i);
                }
            }

            GUILayout.EndScrollView();

            
        }
   
        private static EntityData GetEntityData()
        {               
            EntityData[] foundObjects = Resources.LoadAll<EntityData>("");

            bool dataExists = foundObjects.Length > 0;
            
            if (!dataExists)
            {
                EntityData asset = ScriptableObject.CreateInstance<EntityData>();
                AssetDatabase.CreateAsset(asset,
                    "Assets/DialogueSystem/Editor/Resources/EntityData.asset");
            }
            
            return Resources.Load<EntityData>("EntityData");
        }

        private void OnEnable()
        {
            _entityData = GetEntityData();
        }

        /// <summary>
        /// Draw a slot for a entity
        /// </summary>
        /// <param name="info">Info</param>
        /// <param name="index">Index of the list</param>
        private void DrawEntityDataSlot(EntityInfo info, int index)
        {
            Undo.RecordObject(_entityData, "Modify Entity Data");

            GUIStyle contentStyle = new GUIStyle(GUI.skin.label);
            contentStyle.normal.background = EditorAddOns.MakeTexture(1, 1, new Color(0.3f, 0.3f, 0.3f)); // Set the background color here
            contentStyle.margin = new RectOffset(0, 0, 0, 0);

            GUILayout.BeginVertical(contentStyle);

            #region Entity Content
            GUILayout.Space(10);
            GUILayout.BeginHorizontal(GUILayout.Height(100));
            GUILayout.Space(10);
            GUILayout.BeginVertical();

            // Region: Entity Name
            #region Entity Name
            info.EntityName = EditorGUILayout.TextField(info.EntityName,
                GUILayout.Width(Math.Min(300, position.width - 120)));
            #endregion

            GUILayout.FlexibleSpace();

            // Region: Buttons
            #region Buttons
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Open Settings"))
            {
                ExpressionWindow.ShowWindow(info.Expressions, index, _entityData);
            }

            GUIStyle redButtonStyle = new GUIStyle(GUI.skin.button);

            redButtonStyle.normal.textColor = Color.white; // Set text color to white
            redButtonStyle.fixedWidth = position.width * 0.1f;
            redButtonStyle.alignment = TextAnchor.MiddleCenter; // Center the text in the button

            if (GUILayout.Button($"X", redButtonStyle))
            {
                bool confirmed = EditorUtility.DisplayDialog("Confirm Removal", "Are you sure you want to remove the preset?", "Yes", "No");
                if (confirmed)
                {
                    // Remove the preset when "Yes" is clicked
                    _entityData.RemovePresetAt(index);
                }
            }

            GUILayout.EndHorizontal();
            #endregion

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();

            // Region: Entity Sprite
            #region Entity Sprite
            GUILayout.BeginVertical();
            info.Expressions =
               (ExpressionPreset)EditorGUILayout.ObjectField(obj: info.Expressions,
               objType: typeof(ExpressionPreset), false,
               GUILayout.Height(20), GUILayout.Width(100));

            Expression defaultExpression = null;

            if (((info.Expressions?.Emotions?.Count) ?? -1) > 0)
            {
                defaultExpression = info.Expressions.Emotions[0];
            }

            Sprite defaultSprite = null;
            if (defaultExpression != default)
            {
                defaultSprite = defaultExpression.Image;
            }

            EditorGUILayout.ObjectField(obj: defaultSprite,
                objType: typeof(Sprite), false,
                GUILayout.Height(100), GUILayout.Width(100));
            GUILayout.EndVertical();
            #endregion

            GUILayout.Space(10);

            GUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(5);
            GUILayout.EndVertical();

            EditorUtility.SetDirty(_entityData);
        }

    }
  
}
