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

    private void Start()
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
        battleWinPercentageAmountTMP.text = "" + stats.battlesWon / (stats.battlesWon + stats.battlesLost) + "%";
        battlesForfeitedAmountTMP.text = "" + stats.battlesForfeited;
    }
}
