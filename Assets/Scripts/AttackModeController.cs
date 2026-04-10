using System.Collections;
using TMPro;
using UnityEngine;

public class AttackModeController : MonoBehaviour
{
    public UI_Controller UIController;
    public GameObject battleUI;
    public JadeaWinningScreen battleWinningScreen;
    public StatsController statsController;
    public bool battleIsLost;
    public TMP_Text battleTimeText;
    public TMP_Text rewardsText;
    private int secondsInBattle;

    public void StartAttack()
    {
        InvokeRepeating("CheckForEnemies", 1f, 1f);
    }

    private void CheckForEnemies()
    {
        if (battleIsLost) return;
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            CancelInvoke("CheckForEnemies");
            WinBattle();
        }
    }

    public void LoseBattle()
    {
        CancelInvoke("CheckForEnemies");
        StopAllCoroutines();
        battleUI.SetActive(false);

        if (UIController != null)
        {
            // Disable battle popups
            foreach (GameObject obj in UIController.battleUIPopUps) obj.SetActive(false);
            UIController.optionsMenu.gameObject.SetActive(false);
        }

        if (secondsInBattle < 16) statsController.battlesForfeited++;
        else statsController.battlesLost++;

        statsController.SaveStats();
    }

    private void WinBattle()
    {
        if (UIController != null)
        {
            // Disable battle popups
            foreach (GameObject obj in UIController.battleUIPopUps) obj.SetActive(false);
            UIController.optionsMenu.gameObject.SetActive(false);
        }

        battleTimeText.text = GetBattleTimerString(secondsInBattle);
        StopCoroutine(SecondCounter());
        StartCoroutine(PlayRewardsRisingAnimation());
        battleUI.SetActive(false);
        statsController.totalRewardPercentages += (float)100 * GetTotalRewards() / 1; // change 1 to maxReward

        if (battleWinningScreen != null) battleWinningScreen.gameObject.SetActive(true);

        statsController.battlesWon++;
        statsController.SaveStats();
    }

    private int GetTotalRewards()
    {
        /*if (secondsInBattle <= threatLevel.rewardTimerMin) return threatLevel.maxReward;
        else if (secondsInBattle >= threatLevel.rewardTimerMax) return threatLevel.minReward;
        return Mathf.RoundToInt(threatLevel.maxReward - (threatLevel.maxReward - threatLevel.minReward) *
            ((float)secondsInBattle - threatLevel.rewardTimerMin) / (threatLevel.rewardTimerMax - threatLevel.rewardTimerMin));*/
        return 0;
    }

    private IEnumerator PlayRewardsRisingAnimation()
    {
        int tempRewards = 0;
        int totalRewardsNumber = GetTotalRewards();
        statsController.totalMoneyEarned += totalRewardsNumber;
        statsController.availableMoney += totalRewardsNumber;
        while (tempRewards < totalRewardsNumber)
        {
            rewardsText.text = tempRewards.ToString();
            tempRewards += 1;
            yield return new WaitForSeconds(2.5f / totalRewardsNumber);
        }
        if (tempRewards <= totalRewardsNumber)
        {
            rewardsText.text = totalRewardsNumber.ToString();
        }
    }

    private IEnumerator SecondCounter()
    {
        secondsInBattle = 0;
        while (!battleWinningScreen.gameObject.activeSelf)
        {
            yield return new WaitForSecondsRealtime(1f);
            secondsInBattle++;
        }
    }

    public string GetBattleTimerString(int seconds)
    {
        return string.Format("{0:00}:{1:00}", seconds / 60, seconds % 60);
    }
}
