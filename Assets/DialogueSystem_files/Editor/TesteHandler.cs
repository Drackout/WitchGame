using UnityEditor;
using UnityEngine;
using DialogueSystem;
using DialogueSystem.Editor;

//https://answers.unity.com/questions/634110/associate-my-custom-asset-with-a-custom-editorwind.html
public class TesteHandler : MonoBehaviour
{
    
    [UnityEditor.Callbacks.OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        string assetPath = AssetDatabase.GetAssetPath(instanceID);
        DialogueScript scriptableObject = AssetDatabase.LoadAssetAtPath<DialogueScript>(assetPath);
        if (scriptableObject != null)
        {
            DialogueGraph.OpenDialogueGraphWindow(scriptableObject);
            return true;
        }
        return false; //let unity open it.
    }
}