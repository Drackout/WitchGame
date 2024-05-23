using UnityEngine;

[System.Serializable]
public class SerializedObject
{

    [field: SerializeField]
    public string Type { get; set; }

    public SerializedObject(object obj)
    {
        if(obj is int)
        {
            Type = "int";
            IntValue = (int)obj;
        }
    }

    [field: SerializeField]
    public int IntValue {get; set; }
    public float FloatValue {get; set; }
    public string StringValue {get; set; }
    public bool BoolValue {get; set; }

    public object GetValue()
    {
        if (Type == "int")
        {
            return (object)IntValue;
        }
        else if (Type == "float")
        {
            return (object)FloatValue;
        }
        else if (Type == "string")
        {
            return (object)StringValue;
        }
        else if (Type == "bool")
        {
            return (object)BoolValue;
        }
        else
        {
            return default;
        }

    }
}


