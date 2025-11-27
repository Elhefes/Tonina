using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class WorldSaveLoad
{
    private static string path1 = Application.persistentDataPath + "/world1Data.imox";
    private static string path2 = Application.persistentDataPath + "/world2Data.imox";
    private static string path3 = Application.persistentDataPath + "/world3Data.imox";

    public static void SaveWorldData(WorldData data)
    {
        string path;
        if (PlayerPrefs.GetInt("selectedSaveFile", 1) == 3) path = path3;
        else if (PlayerPrefs.GetInt("selectedSaveFile", 1) == 2) path = path2;
        else path = path1;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static WorldData LoadWorldData()
    {
        string path;
        if (PlayerPrefs.GetInt("selectedSaveFile", 1) == 3) path = path3;
        else if (PlayerPrefs.GetInt("selectedSaveFile", 1) == 2) path = path2;
        else path = path1;

        if (!File.Exists(path))
            return null;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        WorldData data = formatter.Deserialize(stream) as WorldData;
        stream.Close();

        return data;
    }

    public static void DeleteSave(int saveSlot)
    {
        string path;
        if (saveSlot == 3) path = path3;
        else if (saveSlot == 2) path = path2;
        else path = path1;

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.Log("No save file found to delete.");
        }

        if (saveSlot == 3) PlayerPrefs.DeleteKey("playerName3");
        else if (saveSlot == 2) PlayerPrefs.DeleteKey("playerName2");
        else if (saveSlot == 1) PlayerPrefs.DeleteKey("playerName1");

        Debug.Log("All save data cleared from" + path);
    }

}
