[System.Serializable]
public class PyramidProgressData
{
    // There are 25 threat levels, threat progression value that starts from 0 and goes up to 2500 and unlock next level every 100
    // After a battle finishes, there's a float scoreFactor between 0 and 1 based on how well battle went
    // Threat progression formula: (100 - threatLevel * 2) * scoreFactor, when threat level is highest
    // Otherwise, it's (100 / (highestThreatLevel - threatLevel) * 22) * scoreFactor
    // Losing should lower threatProgressionValue by 33 - highestThreatLevel
    // threatProgressionValue minimum value should be maxThreatLevel * 100 - 500
    public float threatProgressionValue { get; set; }
    public int highestThreatLevel { get; set; } // From 3 to 25

    public int highestUnlockedPyramidFloor { get; set; }

    public int maizeProductionLevel { get; set; } // How many maize pieces can player start a battle with, maybe also makes Maize Places cheaper?

    // Placeable buildings
    public bool fenceUnlocked { get; set; }
    public bool maizePlaceUnlocked { get; set; }
    public bool spearRackUnlocked { get; set; }
    public bool fillOkilUnlocked { get; set; }
    public bool towerUnlocked { get; set; }
}
