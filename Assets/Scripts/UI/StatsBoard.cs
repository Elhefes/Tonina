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
        
        pyramidLevelsBuiltAmountTMP.text = "" + stats.pyramidLevelsBuilt + "/14";
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
