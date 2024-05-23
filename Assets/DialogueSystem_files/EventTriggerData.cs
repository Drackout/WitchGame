using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


/// <summary>
/// Stored data related to dialogue events
/// </summary>
[System.Serializable]
public class EventTriggerData
{
    [SerializeField]
    private string uniqueID;
    public string UniqueID => uniqueID;

    [SerializeField]
    private string savedID;
    public string SavedID => savedID;
    
    [SerializeField]
    string functionName;
    public string FunctionName => functionName;

    [SerializeField]
    int indexPos = default;
    public int IndexPos => indexPos;

    public System.Type SelectedComponent => System.Type.GetType
        (
          Assembly.CreateQualifiedName(
              listOfAssemblies[seletedTypeIndex],
              listOfType[seletedTypeIndex])
        );

    public string ty => listOfType[seletedTypeIndex];
    public string ass => listOfAssemblies[seletedTypeIndex];

  


    #region Editor Only
    [SerializeField] [HideInInspector]
    bool showText;
    [SerializeField] [HideInInspector]
    bool showSelf;
    [SerializeField] [HideInInspector]
    List<string> listOfType;
    [SerializeField] [HideInInspector]
    List<string> listOfAssemblies;
    [SerializeField] [HideInInspector]
    int seletedTypeIndex;
    [SerializeField] [HideInInspector]
    int seletedMethodIndex;
    #endregion
}
