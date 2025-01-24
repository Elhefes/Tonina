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

    // Save and Load for ProgressionData
    public static void SaveProgressionData(ProgressionData data)
    {
        string filePath = persistentPath + "/progression.imox";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ProgressionData LoadProgressionData()
    {
        string filePath = persistentPath + "/progression.imox";

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);
            ProgressionData data = formatter.Deserialize(stream) as ProgressionData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found: progression.imox");
            return null;
        }
    }

    private static string GetEditorPathExtension()
    {
        return Application.isEditor ? "Editor" : "";
    }
}
