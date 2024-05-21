using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    private SaveData saveData;

    private string SavePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

    public static SaveManager Instance => instance;
    public SaveData SaveData => saveData;

    public void Save()
    {
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(SavePath, json);
    }

    private void Load()
    {
        if (!File.Exists(SavePath))
        {
            saveData = new SaveData();
            Save();
        }
        else
        {
            string json = File.ReadAllText(SavePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        Load();
    }
}
