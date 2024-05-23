
using System;

namespace DialogueSystem 
{
    [System.Serializable]
    public abstract class CustomFunction
    {
        [field:UnityEngine.SerializeField]
        public string GUID { get; set; }
        public Action onUpdate { get; set; }
        
        #if UNITY_EDITOR
        public virtual void OnEnable() { }
        public abstract void Draw();
        #endif
        public abstract void Invoke();
    }
}
