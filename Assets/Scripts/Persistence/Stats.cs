[System.Serializable]
public class Stats
{
    public int pyramidLevelsBuilt { get; set; }
    public int availableMoney { get; set; }
    public int totalMoneyEarned { get; set; }
    public int enemiesKilled { get; set; }
    public int changesToBattlefield { get; set; }
    public int battlesWon { get; set; }
    public int battlesLost { get; set; }
    public int battlesForfeited { get; set; }

    public Stats(StatsController statsController)
    {
        pyramidLevelsBuilt = statsController.pyramidLevelsBuilt;
        availableMoney = statsController.availableMoney;
        totalMoneyEarned = statsController.totalMoneyEarned;
        enemiesKilled = statsController.enemiesKilled;
        changesToBattlefield = statsController.changesToBattlefield;
        battlesWon = statsController.battlesWon;
        battlesLost = statsController.battlesLost;
        battlesForfeited = statsController.battlesForfeited;
    }
}
