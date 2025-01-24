using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingsData
{
    public int pyramidFloorsBuilt;
    public List<PlaceableBuildingData> placeableBuildings = new List<PlaceableBuildingData>();

    public int buildingCount { get; set; }
    public int maxPlaceableBuildings => pyramidFloorsBuilt * 2 - 3;
}

public class PlaceableBuildingData
{
    public GameObject obj;
    public Vector3 position;
    public Quaternion rotation;

    public PlaceableBuildingData(GameObject obj, Vector3 position, Quaternion rotation)
    {
        this.obj = obj;
        this.position = position;
        this.rotation = rotation;
    }
}
