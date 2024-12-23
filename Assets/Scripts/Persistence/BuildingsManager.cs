using UnityEngine;

public class BuildingsManager : MonoBehaviour
{
    public bool fenceUnlocked;
    public bool maizePlaceUnlocked;
    public bool spearRackUnlocked;
    public bool fillOkilUnlocked;
    public bool towerUnlocked;

    public int buildingsPlaced;
    public int maxBuildingAmount;

    private void Start()
    {
        maxBuildingAmount = 3; // TODO: this value should be pyramidLevelsAmount * 2 - 3
    }
}
