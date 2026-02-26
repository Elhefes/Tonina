using System;

[Serializable]
public class ProgressionData
{
    public float threatProgressionValue { get; set; }
    public float highestThreatValue { get; set; } // Highest progression value that has been achieved
    public int maizeProductionLevel { get; set; } // How many maize pieces can player start a battle with, maybe also makes Maize Places cheaper?

    // Unlocks
    public bool introPlayed { get; set; }
    public bool buildModeGuided { get; set; }
    public bool attackModeUnlocked { get; set; }
    public bool fenceUnlocked { get; set; }
    public bool maizePlaceUnlocked { get; set; }
    public bool spearRackUnlocked { get; set; }
    public bool fillOkillUnlocked { get; set; }
    public bool towerUnlocked { get; set; }

    public bool spearUnlocked { get; set; }
    public bool axeUnlocked { get; set; }
    public bool bowUnlocked { get; set; }

    // Pyramid floors
    public int extraPyramidFloorsBuilt { get; set; }

    // // Weather Temple, Maize Farmers House... etc.
    public int specialBuildingsBuilt { get; set; }
}
