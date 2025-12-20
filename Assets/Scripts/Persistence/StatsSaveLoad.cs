using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class StatsSaveLoad
{
    private static string path1 = Application.persistentDataPath + "/stats1.imox";
    private static string path2 = Application.persistentDataPath + "/stats2.imox";
    private static string path3 = Application.persistentDataPath + "/stats3.imox";

    public static void Save(Stats stats)
    {
        string path;
        if (PlayerPrefs.GetInt("selectedSaveFile", 1) == 3) path = path3;
        else if (PlayerPrefs.GetInt("selectedSaveFile", 1) == 2) path = path2;
        else path = path1;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, stats);
        stream.Close();
    }

    public static Stats Load()
    {
        string path;
        if (PlayerPrefs.GetInt("selectedSaveFile", 1) == 3) path = path3;
        else if (PlayerPrefs.GetInt("selectedSaveFile", 1) == 2) path = path2;
        else path = path1;

        if (!File.Exists(path)) return null;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        Stats s = formatter.Deserialize(stream) as Stats;
        stream.Close();
        return s;
    }

    public static void DeleteStats(int saveSlot)
    {
        string path;
        if (saveSlot == 3) path = path3;
        else if (saveSlot == 2) path = path2;
        else path = path1;

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Stats deleted.");
        }
        else
        {
            Debug.Log("No stats found to delete.");
        }

        Debug.Log("All stats cleared from" + path);
    }
}
