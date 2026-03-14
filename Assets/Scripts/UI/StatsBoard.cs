using System;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Globalization;

public class StatsBoard : MonoBehaviour
{
    private Stats stats;

    private Coroutine timerCoroutine;

    public TMP_Text totalPlayTimeTMP;
    public TMP_Text pyramidLevelsBuiltAmountTMP;
    public TMP_Text pyramidHeightAmountTMP; // Each level adds 5m or 7.5m of height. Also needs m to ft conversion?
    public TMP_Text availableMoneyAmountTMP;
    public TMP_Text averageRewardPercentagesTMP;
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

    private void UpdateStatsMenuTexts()
    {
        if (stats == null) return;
        
        pyramidLevelsBuiltAmountTMP.text = "" + (3 + GameState.Instance.progressionData.extraPyramidFloorsBuilt) + "/8";
        pyramidHeightAmountTMP.text = GetPyramidHeight();
        availableMoneyAmountTMP.text = "" + stats.availableMoney + " gold";
        averageRewardPercentagesTMP.text = GetTotalRewardPercentages();
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
    "40 meters | 131.2 ft",
    "45 meters | 147.6 ft"
};

    private string GetPyramidHeight()
    {
        int extraFloors = GameState.Instance.progressionData.extraPyramidFloorsBuilt;
        extraFloors = Mathf.Clamp(extraFloors, 0, pyramidHeights.Length - 1);
        return pyramidHeights[extraFloors];
    }

    private string GetTotalRewardPercentages()
    {
        if (stats.totalRewardPercentages == 0 || stats.battlesWon == 0) return "0.0%";
        float avg = stats.totalRewardPercentages / stats.battlesWon;
        return avg.ToString("F1", CultureInfo.InvariantCulture) + "%";
    }

    private IEnumerator TimedSecondsUpdate()
    {
        while (gameObject.activeInHierarchy)
        {
            TimeSpan time = TimeSpan.FromSeconds(PlayerPrefs.GetInt("secondsPlayed", 0));

            string timePlayed = time.Hours.ToString("00") + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
            totalPlayTimeTMP.text = timePlayed;

            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
