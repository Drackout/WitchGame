using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Inspector Window for Dialogue Edges
    /// </summary>
    public class DialogueEdgeInspector : ScriptableObject
    {
        //Data related to the choice the edge is connected to
        [SerializeField] 
        public ChoiceData _choiceData;

        //The edge object
        [SerializeField]
        private Edge _edge;
        
        /// <summary>
        /// Function responsible for initializing the Inspector
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="choiceData"></param>
        public void init(Edge edge, ChoiceData choiceData)
        {
            _choiceData = choiceData;
            _edge = edge;
        }
    }
}