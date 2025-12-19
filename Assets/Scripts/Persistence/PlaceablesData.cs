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
    public SerializableVector3 position;
    public SerializableQuaternion rotation;

    public PlaceableData(string name, Vector3 pos, Quaternion rot)
    {
        prefabName = name;
        position = new SerializableVector3(pos);
        rotation = new SerializableQuaternion(rot);
    }

    public Vector3 GetPosition() => position.ToVector3();
    public Quaternion GetRotation() => rotation.ToQuaternion();
}

[Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(float x, float y, float z)
    {
        this.x = x; this.y = y; this.z = z;
    }

    public SerializableVector3(Vector3 v)
    {
        x = v.x; y = v.y; z = v.z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}

[Serializable]
public struct SerializableQuaternion
{
    public float x, y, z, w;

    public SerializableQuaternion(float x, float y, float z, float w)
    {
        this.x = x; this.y = y; this.z = z; this.w = w;
    }

    public SerializableQuaternion(Quaternion q)
    {
        x = q.x; y = q.y; z = q.z; w = q.w;
    }

    public Quaternion ToQuaternion() => new Quaternion(x, y, z, w);
}
