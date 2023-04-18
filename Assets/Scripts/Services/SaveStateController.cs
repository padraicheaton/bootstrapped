using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveStateController
{
    // * Multi-Class Keys
    public static readonly string tutorialCompleteSaveKey = "tutorialComplete";
    public static string masterVolumeValueKey = "setting_masterVolume";
    public static string startingWeaponGenotypeKey = "startingWeaponGenotype";

    private static Dictionary<string, string> saveData = new Dictionary<string, string>();

    private static string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "config.json");
    }

    private static string GetWeaponDataFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "data.json");
    }

    public static void LoadSaveData()
    {
        if (File.Exists(GetFilePath()))
        {
            string jsonString = File.ReadAllText(GetFilePath());
            saveData = FromJsonToDictionary(jsonString);
        }
        else
            Debug.Log("File does not exist");

        LoadEvolutionaryData();
    }

    public static void ClearSaveData()
    {
        if (File.Exists(GetFilePath()))
            File.Delete(GetFilePath());

        if (File.Exists(GetWeaponDataFilePath()))
            File.Delete(GetWeaponDataFilePath());

        saveData = new Dictionary<string, string>();

        EvolutionaryData[] emptyData = { };

        WeaponDataCollector.LoadEvolutionaryData(emptyData);
    }

    public static void SaveDataToFile()
    {
        string jsonString = ToJson(saveData);
        File.WriteAllText(GetFilePath(), jsonString);

        SaveEvolutionaryDataToFile();
    }

    public static void SaveEvolutionaryDataToFile()
    {
        string jsonString = JsonHelper.ToJson(WeaponDataCollector.GetEvolutionaryData());
        File.WriteAllText(GetWeaponDataFilePath(), jsonString);
    }

    private static void LoadEvolutionaryData()
    {
        if (File.Exists(GetWeaponDataFilePath()))
        {
            string jsonString = File.ReadAllText(GetWeaponDataFilePath());
            WeaponDataCollector.LoadEvolutionaryData(JsonHelper.FromJson<EvolutionaryData>(jsonString));
        }
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
                return (T)System.Convert.ChangeType(saveData[key], typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
        return default(T);
    }

    public static bool DatabaseContains(string key)
    {
        return saveData.ContainsKey(key);
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
        jsonString = jsonString.Substring(1, jsonString.Length - 2);

        // This is the case if there is no save data
        if (jsonString.Length == 0) return new Dictionary<string, string>();

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
