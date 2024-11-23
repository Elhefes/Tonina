using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveLoad
{
    private static string persistentPath = Application.persistentDataPath;

    public static void SaveStats(Stats stats)
    {
        string filePath = persistentPath + "/stats" + GetEditorPathExtension() + ".imox";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Create);

        formatter.Serialize(stream, stats);
        stream.Close();
    }

    public static Stats LoadStats()
    {
        string filePath = persistentPath + "/stats" + GetEditorPathExtension() + ".imox";

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            Stats data = formatter.Deserialize(stream) as Stats;
            stream.Close();

            return data;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Save file not found in /stats" + GetEditorPathExtension() + ".imox");
#endif
            return null;
        }
    }

    public static void DeleteMapsData()
    {
        File.Delete(persistentPath + "/mapsData" + GetEditorPathExtension() + ".imox");
    }

    public static void DeleteStats()
    {
        File.Delete(persistentPath + "/stats" + GetEditorPathExtension() + ".imox");
    }

    public static string GetEditorPathExtension()
    {
        return Application.isEditor ? "Editor" : "";
    }

}
