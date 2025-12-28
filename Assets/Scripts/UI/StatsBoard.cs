using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class StatsBoard : MonoBehaviour
{
    private Stats stats;

    private Coroutine timerCoroutine;

    public TMP_Text totalPlayTimeTMP;
    public TMP_Text pyramidLevelsBuiltAmountTMP;
    public TMP_Text pyramidHeightAmountTMP; // Each level adds 5m or 7.5m of height. Also needs m to ft conversion?
    public TMP_Text availableMoneyAmountTMP;
    public TMP_Text enemiesKilledAmountTMP;
    public TMP_Text changesToBattlefieldAmountTMP;
    public TMP_Text battlesWonAmountTMP;
    public TMP_Text battlesLostAmountTMP;
    public TMP_Text battleWinPercentageAmountTMP;
    public TMP_Text battlesForfeitedAmountTMP;

    private void OnEnable()
    {
        stats = StatsSaveLoad.Load();
        UpdateStatsMenuTexts();
        timerCoroutine = StartCoroutine(TimedSecondsUpdate());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void UpdateStatsMenuTexts()
    {
        if (stats == null) return;
        
        pyramidLevelsBuiltAmountTMP.text = "" + (3 + GameState.Instance.progressionData.extraPyramidFloorsBuilt) + "/14";
        pyramidHeightAmountTMP.text = GetPyramidHeight();
        availableMoneyAmountTMP.text = "" + stats.availableMoney + " gold";
        enemiesKilledAmountTMP.text = "" + stats.enemiesKilled;
        changesToBattlefieldAmountTMP.text = "" + stats.changesToBattlefield;
        battlesWonAmountTMP.text = "" + stats.battlesWon;
        battlesLostAmountTMP.text = "" + stats.battlesLost;
        int percentage;
        if (stats.battlesWon > 0)
        {
            if (stats.battlesLost == 0)
            {
                percentage = 100;
            }
            else percentage = Mathf.RoundToInt((float)stats.battlesWon / (stats.battlesWon + stats.battlesLost) * 100);
        }
        else percentage = 0;
        battleWinPercentageAmountTMP.text = "" + percentage + "%";
        battlesForfeitedAmountTMP.text = "" + stats.battlesForfeited;
    }

    private static readonly string[] pyramidHeights =
{
    "15 meters | 49.2 ft",
    "20 meters | 65.6 ft",
    "27.5 meters | 90.2 ft",
    "32.5 meters | 106.6 ft",
    "37.5 meters | 123.0 ft",
    "42.5 meters | 139.4 ft",
    "47.5 meters | 155.8 ft",
    "55 meters | 180.4 ft",
    "60 meters | 196.9 ft",
    "65 meters | 213.2 ft",
    "70 meters | 229.7 ft",
    "75 meters | 246.1 ft"
};

    private string GetPyramidHeight()
    {
        int extraFloors = GameState.Instance.progressionData.extraPyramidFloorsBuilt;
        extraFloors = Mathf.Clamp(extraFloors, 0, pyramidHeights.Length - 1);
        return pyramidHeights[extraFloors];
    }


    IEnumerator TimedSecondsUpdate()
    {
        while (gameObject.activeInHierarchy)
        {
            stats = StatsSaveLoad.Load();
            TimeSpan time = TimeSpan.FromSeconds(stats.secondsPlayed);

            string timePlayed = time.TotalHours.ToString("00") + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
            totalPlayTimeTMP.text = timePlayed;

            yield return new WaitForSecondsRealtime(0.25f);
        }
    }
}
