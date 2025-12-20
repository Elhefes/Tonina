using System.Collections;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    // Stats
    public int secondsPlayed { get; set; }
    public int pyramidLevelsBuilt { get; set; }
    public int availableMoney { get; set; }
    public int totalMoneyEarned { get; set; }
    public int enemiesKilled { get; set; }
    public int changesToBattlefield { get; set; }
    public int battlesWon { get; set; }
    public int battlesLost { get; set; }
    public int battlesForfeited { get; set; }

    private void Start()
    {
        LoadStats();
        StartCoroutine(SecondCounter());
    }

    public void SaveStats()
    {
        Stats stats = new Stats(this);
        StatsSaveLoad.Save(stats);
    }

    public void LoadStats()
    {
        Stats stats = StatsSaveLoad.Load();
        if (stats == null) return;
        this.secondsPlayed = stats.secondsPlayed;
        this.pyramidLevelsBuilt = stats.pyramidLevelsBuilt;
        this.availableMoney = stats.availableMoney;
        this.totalMoneyEarned = stats.totalMoneyEarned;
        this.enemiesKilled = stats.enemiesKilled;
        this.changesToBattlefield = stats.changesToBattlefield;
        this.battlesWon = stats.battlesWon;
        this.battlesLost = stats.battlesLost;
        this.battlesForfeited = stats.battlesForfeited;
    }

    IEnumerator SecondCounter()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            secondsPlayed++;
            SaveStats();
        }
    }
}
