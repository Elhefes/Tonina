using UnityEngine;
using TMPro;

public class StatsBoard : MonoBehaviour
{
    private Stats stats;

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
        stats = SaveLoad.LoadStats();
        UpdateStatsMenuTexts();
    }

    void UpdateStatsMenuTexts()
    {
        if (stats == null) return;
        pyramidLevelsBuiltAmountTMP.text = "" + stats.pyramidLevelsBuilt + "/14";
        availableMoneyAmountTMP.text = "" + stats.availableMoney;
        enemiesKilledAmountTMP.text = "" + stats.enemiesKilled;
        changesToBattlefieldAmountTMP.text = "" + stats.changesToBattlefield;
        battlesWonAmountTMP.text = "" + stats.battlesWon;
        battlesLostAmountTMP.text = "" + stats.battlesLost;
        int percentage;
        if (stats.battlesLost == 0 && stats.battlesWon != 0) percentage = 100;
        else percentage = Mathf.RoundToInt((float)stats.battlesWon / (stats.battlesWon + stats.battlesLost) * 100);
        print(Mathf.RoundToInt((float)stats.battlesWon / (stats.battlesWon + stats.battlesLost) * 100));
        battleWinPercentageAmountTMP.text = "" + percentage + "%";
        battlesForfeitedAmountTMP.text = "" + stats.battlesForfeited;
    }
}
