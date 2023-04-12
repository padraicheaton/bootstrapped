using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveStateController
{
    // * Multi-Class Keys
    public static readonly string tutorialCompleteSaveKey = "tutorialComplete";

    private static Dictionary<string, string> saveData = new Dictionary<string, string>();

    private static string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "config.json");
    }

    public static void LoadSaveData()
    {
        if (File.Exists(GetFilePath()))
        {
            string jsonString = File.ReadAllText(GetFilePath());
            saveData = FromJsonToDictionary(jsonString);
        }
    }

    public static void ClearSaveData()
    {
        saveData = new Dictionary<string, string>();
        SaveDataToFile();
    }

    public static void SaveDataToFile()
    {
        string jsonString = ToJson(saveData);
        File.WriteAllText(GetFilePath(), jsonString);
    }

    public static void SetData(string key, object value)
    {
        saveData[key] = value.ToString();
        SaveDataToFile();
    }

    public static T GetData<T>(string key)
    {
        if (saveData.ContainsKey(key))
        {
            try
            {
                Debug.Log(key);
                Debug.Log(saveData[key]);
                return (T)System.Convert.ChangeType(saveData[key], typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
        return default(T);
    }

    private static void OnApplicationQuit()
    {
        SaveDataToFile();
    }

    private static void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveDataToFile();
        }
    }

    private static string ToJson(Dictionary<string, string> dictionary)
    {
        string json = "{";
        foreach (KeyValuePair<string, string> kvp in dictionary)
        {
            json += $"\"{kvp.Key}\":{kvp.Value},";
        }
        if (json.EndsWith(","))
        {
            json = json.Remove(json.Length - 1);
        }
        json += "}";
        return json;
    }

    private static Dictionary<string, string> FromJsonToDictionary(string jsonString)
    {
        Debug.Log(jsonString);
        jsonString = jsonString.Substring(1, jsonString.Length - 2);
        Debug.Log(jsonString);

        // This is the case if there is no save data
        if (jsonString.Length == 0) return null;

        Dictionary<string, string> temp = new Dictionary<string, string>();

        foreach (string kvpString in jsonString.Split(","))
        {
            string key = kvpString.Split(":")[0].Trim('\"');
            string value = kvpString.Split(":")[1].Trim('\"');

            temp.Add(key, value);
        }


        return temp;
    }
}
