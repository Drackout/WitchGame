using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueSystem
{
    public class DialogueUniqueId : MonoBehaviour
    {
        [SerializeField]
        private string uniqueID;
        public string UniqueID { get => uniqueID; set => uniqueID = value; }
        public int TimesUsed => UsedIn.Count;
        public List<DialogueScript> UsedIn { get => usedIn; set => usedIn = value; }

        [SerializeField]
        private List<DialogueScript> usedIn;

    }
}