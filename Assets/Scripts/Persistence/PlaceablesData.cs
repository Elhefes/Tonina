using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlaceablesData
{
    public int placeablesCount;
    public List<PlaceableData> placeables = new List<PlaceableData>();
}

[Serializable]
public class PlaceableData
{
    public string prefabName;
    public Vector3 position;
    public Quaternion rotation;

    public PlaceableData(string name, Vector3 pos, Quaternion rot)
    {
        prefabName = name;
        position = pos;
        rotation = rot;
    }
}
