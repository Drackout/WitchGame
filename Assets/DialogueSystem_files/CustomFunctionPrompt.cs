using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Temporary Custom Prompt for handling editor 
    /// </summary>
    public class CustomFunctionPrompt : CustomFunction
    {
        //Represents the custom function selected by the user
        private int _selectedClassIndex = 0;
        
        /// <summary>
        /// Method trigered after selecting a new function in the drop down
        /// </summary>
        public Action<CustomFunction> onSelectFunction { get; set; }


        #if UNITY_EDITOR
        /// <summary>
        /// Override method to draw the Function in the inspector
        /// Uses Layout 
        /// </summary>
        public override void Draw()
        {
            List<Type> functionTypes = 
                FindInheritedClasses<CustomFunction>().ToList();


            // Create an array to store the names of the inheriting classes
            List<string> classNames = functionTypes.Select(t => t.Name).ToList();

            classNames.Insert(0, "None");

            int o = _selectedClassIndex;

            // Display the dropdown menu with the class names
            _selectedClassIndex = EditorGUILayout.Popup(_selectedClassIndex, classNames.ToArray(), GUILayout.ExpandWidth(true));

            //There was a change on the drop down
            if (o != _selectedClassIndex)
            {
                //Get concrete function
                CustomFunction customFunc = CreateCustomFunction(_selectedClassIndex - 1, functionTypes);

                onSelectFunction?.Invoke(customFunc);
            }
        }
        #endif

        /// <summary>
        /// Override method that represents the action of the function. 
        /// Empty because this is a aux class
        /// </summary>
        public override void Invoke()
        {
        
        }

        /// <summary>
        /// Method to find all types that inherit from a given class
        /// </summary>
        /// <typeparam name="T">Parent class to search for</typeparam>
        /// <returns>List of types</returns>
        private IEnumerable<Type> FindInheritedClasses<T>()
        {
            return Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract).Where(x => x != typeof(CustomFunctionPrompt));
        }
    
        /// <summary>
        /// Create a CustomFunction of a certain type 
        /// </summary>
        /// <param name="index">Index of the CustomFunction selected</param>
        /// <param name="typeList">List of all classes that inherit CustomFunction</param>
        /// <returns>-New CustomFunction</returns>
        public CustomFunction CreateCustomFunction(int index, List<Type> typeList)
        {
            if (index < 0 || index >= typeList.Count)
            {
                Debug.LogError("Index out of range.");
                return null;
            }

            Type selectedType = typeList[index];

            if (!typeof(CustomFunction).IsAssignableFrom(selectedType))
            {
                Debug.LogError("Selected type does not inherit from CustomFunction.");
                return null;
            }

            try
            {
                CustomFunction instance = (CustomFunction)Activator.CreateInstance(selectedType);
                return instance;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to create instance: " + e.Message);
                return null;
            }
        }
    }
}