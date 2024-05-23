using UnityEngine;
using System.Linq;

[System.Serializable]
public class RuntimeEventData
{
    /// <summary>
    /// Constructor for the RuntimeEventData class
    /// </summary>
    /// <param name="classType">Type of the class this event is from</param>
    /// <param name="methodInfo">Method inside the type</param>
    /// <param name="index">Letter this event should be played in</param>
    public RuntimeEventData(System.Type classType, string methodName, int index, object[] parameters = null)
    {
        ClassType = classType;
        MethodName = methodName;
        TriggerIndex = index;
        // if(_params != null)
            _params = parameters.Select(x => new SerializedObject(x)).ToArray();
        // else
        //     _params = new SerializedObject[] { };
    }

    [field: SerializeField]
    public System.Type ClassType { get; private set; }

    [field: SerializeField]
    public string MethodName{ get; private set; }

    [field: SerializeField]
    public int TriggerIndex { get; private set; }

    [field: SerializeField]
    private SerializedObject[] _params;

    [field: SerializeField]
    public object[] Params => _params?.Select( x => x.GetValue()).ToArray();

    #region  Editor

    [HideInInspector] [field: SerializeField]
    public int ClassIndex { get; set; }

    [HideInInspector] [field: SerializeField]
    public int MethodIndex { get; set; }

    [HideInInspector] [field: SerializeField]
    public bool ShowText { get; set; }

    [HideInInspector] [field: SerializeField]
    public bool ShowSelf { get; set; }

    #endregion
}


