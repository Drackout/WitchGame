using System.Collections.Generic;

namespace DialogueSystem
{
    public interface IDialogueScript : IEnumerable<NodeData>
    {
        NodeData this[string key] { get; }
        NodeData this[int index] { get; }

        NodeData GetNextNode(NodeData current, int choice = 0);
    }
}