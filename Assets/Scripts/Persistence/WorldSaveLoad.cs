using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class WorldSaveLoad
{
    private static string path = Application.persistentDataPath + "/worldData.imox";

    public static void SaveWorldData(WorldData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static WorldData LoadWorldData()
    {
        if (!File.Exists(path))
            return null;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        WorldData data = formatter.Deserialize(stream) as WorldData;
        stream.Close();

        return data;
    }
}
