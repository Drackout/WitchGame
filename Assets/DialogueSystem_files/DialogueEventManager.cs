using System.Collections.Generic;
using UnityEngine;


//Maybe transfomr into a Singleton
//For now this works
namespace DialogueSystem 
{
    public class DialogueEventManager : MonoBehaviour, ISerializationCallbackReceiver
    {

        [SerializeField]
        private List<GameObjectIdPar> SerializationParList;
        [SerializeField]
        private List<GameObjectIdPar> TestList;

        public void OnAfterDeserialize()
        {
            parList = SerializationParList;
            tempList = TestList;
        }

        public void OnBeforeSerialize()
        {
            SerializationParList = parList;
            TestList = tempList;
        }

        private void Start()
        {
            CleanData();
        }

        private static void CleanData()
        {
            for (int i1 = parList.Count - 1; i1 >= 0; i1--)
            {
                GameObjectIdPar par = parList[i1];
                for (int i = par.Scripts.Count - 1; i >= 0; i--)
                {
                    if(par.Scripts[i] == null)
                    {
                        par.Scripts.RemoveAt(i);
                    }
                }

                if (par.Scripts.Count == 0)
                    parList.Remove(par);
            }
        }
        public static void ClearTemp()
        {
            tempList?.Clear();
            CleanData();
        }

        private static List<GameObjectIdPar> parList;
        private static List<GameObjectIdPar> tempList;

        [System.Serializable]
        private struct GameObjectIdPar
        {
            string nameOf => gameObject.name;
            [SerializeField]
            GameObject gameObject;
            [SerializeField]
            string id;           
            [SerializeField]
            List<DialogueScript> scripts;


            public GameObjectIdPar(string id, GameObject gameObject)
            {
                scripts = new List<DialogueScript> { };
                this.id = id;
                this.gameObject = gameObject;
            }

            public GameObject GameObject { get => gameObject; private set => gameObject = value; }
            public string Id { get => id; private set => id = value; }
            public List<DialogueScript> Scripts { get => scripts; set => scripts = value; }
        }

        public static string GetID(GameObject newGameObject)
        {
         
            if (GetGameObject(newGameObject) == null && 
                CheckTempList(newGameObject) == null)
            {
                string id = System.Guid.NewGuid().ToString();
                tempList.Add(new GameObjectIdPar(id, newGameObject));

                return id;        
            }

            return GetGameObjectID(newGameObject); 
        }
        


        public static void AddNewGameObject(GameObject newGameObject, DialogueScript script)
        {
            GameObject obj = GetGameObject(newGameObject);
            if (obj != null)
            {
                //Already existing GameObject
                GetPar(newGameObject).Scripts.Add(script);
            }
            else
            {
                //New GameObject
                GameObjectIdPar par = GetTempPar(newGameObject);
                par.Scripts.Add(script);
                parList.Add(par);
            }
        }

        public static void RemoveGameObject(GameObject newGameObject, DialogueScript script)
        {
            GameObject obj = GetGameObject(newGameObject);
            
            if (obj == null) return;
            int index = parList.IndexOf(GetPar(obj));

         
            GameObjectIdPar newPar = parList[index];
            newPar.Scripts.Remove(script);
            parList[index] = newPar;

            //DialogueUniqueId uniqueId = obj.GetComponent<DialogueUniqueId>();
            //uniqueId.UsedIn.Remove(script);
            //if (uniqueId.TimesUsed == 0)
            //    Destroy(uniqueId); 
        }



        public static GameObject GetGameObject(string id)
        {
            foreach (GameObjectIdPar gp in parList)
            {
                if (gp.Id == id)
                    return gp.GameObject;
            }

            return null;
        }

        public static GameObject GetGameObject(GameObject gameObject)
        {
            foreach (GameObjectIdPar gp in parList)
            {
                if (gp.GameObject == gameObject)
                    return gp.GameObject;
            }

            return null;
        }

        private static GameObject CheckTempList(GameObject obj)
        {
            foreach (GameObjectIdPar gp in tempList)
            {
                if (gp.GameObject == obj)
                    return gp.GameObject;
            }

            return null;
        }
        
        private static string GetGameObjectID(GameObject gameObject)
        {
            return GetPar(gameObject).Id;
        }
        


        private static GameObjectIdPar GetPar(GameObject gameObject)
        {
            foreach (GameObjectIdPar gp in parList)
            {
                if (gp.GameObject == gameObject)
                    return gp;
            }

            return default;
        }

        private static GameObjectIdPar GetTempPar(GameObject gameObject)
        {
            foreach (GameObjectIdPar gp in tempList)
            {
                if (gp.GameObject == gameObject)
                    return gp;
            }

            return default;
        }

    }
}