using System;

[Serializable]
public class WorldData
{
    public ProgressionData progression;
    public PlaceablesData placeables;

    public WorldData(ProgressionData p, PlaceablesData b)
    {
        progression = p;
        placeables = b;
    }
}
