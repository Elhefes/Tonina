[System.Serializable]
public class PlaceableBuildingData
{
    public int buildingCount { get; set; }
    public int buildingCountLimit { get; set; }

    public float[] position;
    public float[] rotation;

    public PlaceableBuildingData(PlaceableBuilding building)
    {
        position = new float[3];
        position[0] = building.transform.position.x;
        position[1] = building.transform.position.y;
        position[2] = building.transform.position.z;

        rotation = new float[4];
        rotation[0] = building.transform.rotation.x;
        rotation[1] = building.transform.rotation.y;
        rotation[2] = building.transform.rotation.z;
        rotation[3] = building.transform.rotation.w;
    }
}
