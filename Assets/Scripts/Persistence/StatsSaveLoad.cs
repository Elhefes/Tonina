using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class StatsSaveLoad
{
    private static string path = Application.persistentDataPath + "/stats.imox";

    public static void Save(Stats stats)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, stats);
        stream.Close();
    }

    public static Stats Load()
    {
        if (!File.Exists(path)) return null;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        Stats s = formatter.Deserialize(stream) as Stats;
        stream.Close();
        return s;
    }
}
