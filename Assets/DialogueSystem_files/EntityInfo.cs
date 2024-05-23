using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Class that stores the information of a dialogue entity
    /// </summary>
    [System.Serializable]
    public class EntityInfo
    {
        /// <summary>
        /// Name of the entity. 
        /// </summary>
        [field: SerializeField]
        public string EntityName { get; set; }
        
        /// <summary>
        /// List of expressions the entity can have
        /// </summary>
        [field: SerializeField]
        public ExpressionPreset Expressions { get; set; }

        /// <summary>
        /// Manages if the preset is hidden or not
        /// </summary>
        [field: SerializeField] [HideInInspector]
        public bool Hidden { get; set; }                   
    }
}