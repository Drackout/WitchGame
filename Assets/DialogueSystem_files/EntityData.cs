using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DialogueSystem
{
    //Day 32
    //This class has completly broken me. The data disapears randomly in a diferent way every time.
    //When I try to replicate the error it works correctly. I am being mocked by it.
    //Adding Debug.Log sometimes fixes it, adding a random, not used, string value also fixes it sometimes
    //But the data keeps disapearing. Why. What did I do. 

    //Day 33
    //I think I fixed it for good. I used a wierd unity method in another class that doesn't do what the name says it does.
    //I guess it is expected from unity. I still don't get how this got broken so hard. 
    //Whatever, I'm going to accept that at least it works... for now.
    // Warning: Never use Resources.FindObjectsOfTypeAll, it doesnt find all objects of a certain type inside resources.
    
    //Day 34 
    //It stopped working again.

    //Day 35 
    // I don't get it... The random unused string is the thing that fixes the bug...
    // What
    public class EntityData : ScriptableObject
    {
        public List<string> presetNames => CreateNamesList();

        private List<string> CreateNamesList()
        {
            List<string> returnList =
                data.Select(x => x.EntityName).ToList();
            returnList.Insert(0, "Default");

            return returnList;
        }

        //Don't Delete. I don't know why but this is the thing keeping everything working
        [SerializeField]
        private string randomUnusedString;

        [SerializeField]
        public List<EntityInfo> data;

        private EntityInfo defaultPreset;

        public void AddNewPreset()
        {
            data.Add(new EntityInfo());
        }

        public void RemovePresetAt(int index)
        {
            data.RemoveAt(index);
        }

        public EntityInfo this[string key]
        {
            get
            {
                if(key == "default")
                    return defaultPreset;


                foreach (EntityInfo data in data)
                {
                    if (data.EntityName == key)
                        return data;
                }
                return null;
            }
            private set { }
        }

    }
}