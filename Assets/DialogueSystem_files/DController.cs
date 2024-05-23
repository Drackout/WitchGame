using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "Scriptables/DialogueController")]
    public class DController : ScriptableObject
    {
        [SerializeField]
        private List<DialogueScript> _dialogues;

        [SerializeField]
        private int _defaultDialogue = 0;


        public DialogueScript GetDialogue()
        {
            return _dialogues[_defaultDialogue];
        }

        public void ChangeDefault(int v)
        {
            _defaultDialogue = v;
        }
    }

}
