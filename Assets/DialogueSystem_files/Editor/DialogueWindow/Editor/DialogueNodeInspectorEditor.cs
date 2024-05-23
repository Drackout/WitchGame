using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    [CustomEditor(typeof(DialogueNodeInspector))]
    public class DialogueNodeInspectorEditor : UnityEditor.Editor
    {
        SerializedProperty dialogueText;
        DialogueNodeInspector nodeInsp;

        int funcSelectPopup;

        private List<CustomFunction> _customFuctions;

        void OnEnable()
        {
            nodeInsp = target as DialogueNodeInspector;;
            dialogueText = serializedObject.FindProperty("dialogueText");
            _customFuctions = nodeInsp.CustomFunctions;
            if(_customFuctions == null) _customFuctions= new List<CustomFunction>();
        }

        public override VisualElement CreateInspectorGUI()
        {
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {           
            nodeInsp = target as DialogueNodeInspector;
        
            serializedObject.Update();

            base.OnInspectorGUI();

            //Handle the text of the node
            #region Dialogue Text

            string dialTxtTemp = dialogueText.stringValue; 

            EditorGUILayout.PropertyField(dialogueText, new GUIContent("Dialogue Text"));
           
            //On change text event
            if (dialTxtTemp != dialogueText.stringValue)
                nodeInsp.ChangeDialogue(dialogueText.stringValue);

            #endregion

            GUILayout.Space(10);

            //Make sure the Controller is selected to display the 
            //runtime events
            if(nodeInsp.Controller != null)
            {
                List<RuntimeEventData> events = nodeInsp.Events;

                #region Event Region
                GUILayout.BeginHorizontal();
            
                if(GUILayout.Button("Add Event"))
                {
                    //Insert new element
                    events.Add(new RuntimeEventData(null, null, 0));
                }
            
                if (GUILayout.Button("Clear"))
                {
                    events.Clear();
                }

                GUILayout.EndHorizontal();

                GUILayout.Space(5);
            
                EditorGUI.indentLevel = 2;
        
                //Draw Event Trigger Data
                for (int i = 0; i < events.Count; i++)
                {
                    events[i] = DrawRunTimeEventSlot(events[i], i);
                   
                }

                EditorGUI.indentLevel = 1;
                #endregion           

                EditorUtility.SetDirty(nodeInsp);    
            }
            else
            {
                //If a controller does not exist
                //Display a warning
                GUILayout.Label("Runtime Events can only be assigned when" + 
                 " a Dialogue Controller is selected.");
            }

            GUILayout.Space(10);

            //Display the Custom Events
            #region Custom Events
            if (GUILayout.Button("Add Custom Event"))
            {
                CustomFunctionPrompt prompt = new CustomFunctionPrompt();
                SetupCustomFunction(prompt);
            }

            GUILayout.Space(10);

            for (int i = 0; i < _customFuctions.Count; i++)
            {
                 CustomFunction func = _customFuctions[i];
                 #if UNITY_EDITOR
                 func.Draw();
                 #endif
            }
            #endregion

            serializedObject.ApplyModifiedProperties();
        }

            
        private void DrawEventSlot(SerializedProperty eventInfo, int i)
        {
               #region Assign Serialized Properties
                SerializedProperty functionName =
                   eventInfo.FindPropertyRelative("functionName");

                SerializedProperty index =
                   eventInfo.FindPropertyRelative("indexPos");

                SerializedProperty showText =
                   eventInfo.FindPropertyRelative("showText");

                SerializedProperty showSelf =
                   eventInfo.FindPropertyRelative("showSelf");

                SerializedProperty uniqueID =
                   eventInfo.FindPropertyRelative("uniqueID");

                SerializedProperty savedID =
                  eventInfo.FindPropertyRelative("savedID");

                SerializedProperty listOfType =
                   eventInfo.FindPropertyRelative("listOfType");

                SerializedProperty selectedComponent =
                   eventInfo.FindPropertyRelative("selectedComponent");

                SerializedProperty seletedTypeIndex =
                   eventInfo.FindPropertyRelative("seletedTypeIndex");

                SerializedProperty seletedMethodIndex =
                  eventInfo.FindPropertyRelative("seletedMethodIndex");

                SerializedProperty listOfAssemblies =
                  eventInfo.FindPropertyRelative("listOfAssemblies");

                #endregion

                GUILayout.Space(5);
                
                if (EditorGUILayout.DropdownButton(new GUIContent($"Event { i }"), FocusType.Passive))
                {
                    showSelf.boolValue = !showSelf.boolValue;
                }

                if (showSelf.boolValue)
                {
                    GUILayout.Space(10);

                    #region Show Text Toggle
                    EditorGUILayout.PropertyField(showText);
                    if (showText.boolValue)
                        DrawTriggerLabel(index.intValue, dialogueText.stringValue);
                    #endregion

                    #region Selected Letter Index Trigger
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(28);
                    GUILayout.Label("Index");
                    index.intValue = (int)EditorGUILayout.Slider(index.intValue, 0, dialogueText.stringValue.Length - 1);
                    GUILayout.EndHorizontal();
                    #endregion

                    #region GameObject Selection
                    GameObject gameObjTemp = nodeInsp.Controller.gameObject;

                    if(gameObjTemp != null)
                    {
                        #region Create List of Types and Assemblies
                        Component[] components = gameObjTemp.GetComponents(typeof(Component));
                        List<System.Type> listType = new List<Type> { };
                        foreach (Component component in components)
                        {
                            listType.Add(component.GetType());
                        }

                        listOfType.ClearArray();

                        for (int o = 0; o < listType.Count; o++)
                        {
                            listOfType.InsertArrayElementAtIndex(listOfType.arraySize);
                            listOfAssemblies.InsertArrayElementAtIndex(listOfAssemblies.arraySize);

                            listOfType.GetArrayElementAtIndex(o).stringValue = listType[o].ToString();
                            listOfAssemblies.GetArrayElementAtIndex(o).stringValue =
                                Assembly.GetAssembly(listType[o]).ToString();
                        }
                        #endregion 
                    }
                    else
                    {

                    }

                    #endregion

                    #region Component Type PopUp
                    if (listOfType.arraySize > 0)
                    {
                        string[] typeNameList = new string[listOfType.arraySize];
                        for (int o = 0; o < listOfType.arraySize; o++)
                        {
                            typeNameList[o] = listOfType.GetArrayElementAtIndex(o).stringValue;
                        }

                        seletedTypeIndex.intValue =
                        EditorGUILayout.Popup(new GUIContent("Object Components"), seletedTypeIndex.intValue, typeNameList);
                    }
                    #endregion

                    #region Method PopUp
                    if (listOfType.arraySize > 0)
                    {
                        string assemblyName = listOfAssemblies.GetArrayElementAtIndex(seletedTypeIndex.intValue).stringValue;
                        string type = listOfType.GetArrayElementAtIndex(seletedTypeIndex.intValue).stringValue;
                        string qualifiedName = Assembly.CreateQualifiedName(assemblyName, type);
                        System.Type typeSelected = Type.GetType(qualifiedName);

                        MethodInfo[] methodInfo = typeSelected.GetMethods();
                        string[] methodNames = methodInfo.Select(x => x.Name).ToArray();
                        seletedMethodIndex.intValue =
                            EditorGUILayout.Popup(new GUIContent("Methods"), seletedMethodIndex.intValue, methodNames);
                        functionName.stringValue = methodNames[seletedMethodIndex.intValue];
                    }
                    #endregion

                    #region Unique Id
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    string idField = uniqueID.stringValue != "" ? uniqueID.stringValue : "no id";
                    GUILayout.Label(idField);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    #endregion

                    EditorGUILayout.PropertyField(functionName);


                    GUILayout.Space(10);

                    if (GUILayout.Button("Remove Event"))
                    {
                        //Remove Event From Manager
                        //events.DeleteArrayElementAtIndex(i);
                    }

                }

                GUILayout.Space(10);
        }
       
        private RuntimeEventData DrawRunTimeEventSlot(RuntimeEventData data, int i)
        {
            if (EditorGUILayout.DropdownButton(new GUIContent($"Event { i }"), FocusType.Passive))
            {
                data.ShowSelf = !data.ShowSelf;
            }

            int index = data.TriggerIndex;
            string methodName = data.MethodName;
            System.Type type = data.ClassType;
            object[] parameters = data.Params;
            
            if (data.ShowSelf)
            {
                GUILayout.Space(10);

                //Shows a display of the text to show when the event is to be triggered
                #region Show Text Toggle
                data.ShowText = EditorGUILayout.Toggle(new GUIContent("Toggle Text View"),data.ShowText);
                if (data.ShowText)
                    DrawTriggerLabel(data.TriggerIndex, dialogueText.stringValue);
                #endregion

                //Slider to select in which letter the event is triggered
                #region Selected Letter Index Trigger
                GUILayout.BeginHorizontal();
                GUILayout.Space(28);
                GUILayout.Label("Index");
                index = (int)EditorGUILayout.Slider(index, 0, dialogueText.stringValue.Length - 1);
                GUILayout.EndHorizontal();
                #endregion


                List<System.Type> typeList = new List<Type> { };

                //Setups the controller objects to connect the event to
                #region GameObject Setup
                GameObject gameObjTemp = nodeInsp.Controller.gameObject;


                if(gameObjTemp != null)
                {
                    #region Create List of Types and Assemblies
                    
                    Component[] components = gameObjTemp.GetComponents(typeof(Component));
                                      
                    foreach (Component component in components)
                    {
                        typeList.Add(component.GetType());
                    }

                    #endregion 
                }
                #endregion
            
                //Pop up to select which Component to use
                #region Component Type PopUp
                if (typeList.Count > 0)
                {
                    string[] typeNameList = typeList.Select(x => x.Name).ToArray();
 
                    data.ClassIndex =
                        EditorGUILayout.Popup(new GUIContent("Component"), data.ClassIndex, typeNameList);

                    type = typeList[data.ClassIndex];
                }
                #endregion

                MethodInfo[] methodInfos = type.GetMethods();
               
                #region Method PopUp
                if (typeList.Count > 0)
                {
                   

                    string[] methodNames = methodInfos.Select(x => x.Name).ToArray();

                    data.MethodIndex =
                        EditorGUILayout.Popup(new GUIContent("Method"), data.MethodIndex, methodNames);

                    methodName = methodInfos[data.MethodIndex].Name;
                }
                #endregion
    
                

                #region Paramns
                ParameterInfo[] neededParams = methodInfos[data.MethodIndex].GetParameters();            

                if(neededParams.Length > 0)
                {
                    GUILayout.Label("Params");

                    if ((parameters?.Length ?? 0) != neededParams.Length)
                    {
                        parameters = new object[neededParams.Length];
                    }

                    int it = 0;
                    foreach(ParameterInfo info in neededParams)
                    {
                        parameters[it] = DisplayParam(info, parameters[it]);
                        it++;
                    }
                }

                #endregion

            }

            return new RuntimeEventData(type, methodName, index, parameters)
            {
                MethodIndex = data.MethodIndex,
                ClassIndex = data.ClassIndex,
                ShowSelf = data.ShowSelf,
                ShowText = data.ShowText
            };
        }

        private void SetupCustomFunction(CustomFunctionPrompt prompt)
        {
            int index = _customFuctions.Count;
            _customFuctions.Add(prompt);
            #if UNITY_EDITOR
            prompt.OnEnable();
            #endif

            nodeInsp.UpdateNode();

            prompt.onSelectFunction += (c) =>
            {
                c.GUID = GUID.Generate().ToString();
                
                _customFuctions[index] = c;
               #if UNITY_EDITOR
                c.OnEnable();
                #endif

                nodeInsp.UpdateNode();

                c.onUpdate += () =>
                {
                    nodeInsp.UpdateNode();
                };
            };
        }

        private void DrawTriggerLabel(int intValue, string text)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            
            for (int i = 0; i < text.Length; i++)
            {
                if (i % 25 == 0)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
   
                if (intValue == i)
                {
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.red;
                    style.fontSize = 20;
                    GUILayout.Label(text[i].ToString(), style);
                }
                else
                {
                    GUILayout.Label(text[i].ToString());
                }

             
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
     
        private object DisplayParam(ParameterInfo info, object current)
        {
            System.Type type = info.ParameterType;
            
            if(type == typeof(int))
            {
                int value = current != null ? (int)current : 0;
                return (object)EditorGUILayout.IntField(value);
            }
            else if (type == typeof(string))
            {
                return default;
            }
            else if (type == typeof(float))
            {
                return default;
            }
            else if (type == typeof(bool))
            {
                return default;
            }
            else
            {
                GUILayout.Label("Parameter Type not Implemented");
                return default;
            }
        }
    }
}
