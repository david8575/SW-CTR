using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager
{
    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }
            return instance;
        }
    }

    const string dataFileName = "GameData.json";
    string savePath;

    private GameData saveData;
    public GameData SaveData
    {
        get {
            if (saveData == null)
                LoadGameData();

            return saveData; 
        }
        set { saveData = value; }
    }

    DataManager()
    {
        savePath = Path.Combine(Application.persistentDataPath, dataFileName);
    }

    public void LoadGameData()
    {
        if (File.Exists(savePath))
        {
            Debug.Log("Load Game Data");
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            Debug.Log("Make New Save Data");
            saveData = new GameData();
            SaveGameData();
        }
    }

    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);
    }

    public void RemoveGameData()
    {
        saveData = new GameData();
        SaveGameData();
    }
}
