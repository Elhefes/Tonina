using System.Collections;
using TMPro;
using UnityEngine;

public class AttackWinScreen : MonoBehaviour
{
    public int winReward;
    public TMP_Text battleTimeText;
    public TMP_Text rewardsText;
    public UI_Controller uiController;
    public StatsController statsController;
    private int secondsInBattle;

    public void WinBattle()
    {
        battleTimeText.gameObject.SetActive(true); // This is used with time counting

        if (uiController != null)
        {
            // Disable battle popups
            foreach (GameObject obj in uiController.battleUIPopUps) obj.SetActive(false);
            uiController.optionsMenu.gameObject.SetActive(false);
        }

        battleTimeText.text = GetBattleTimerString(secondsInBattle);
        StopCoroutine(SecondCounter());
        StartCoroutine(PlayRewardsRisingAnimation());

        statsController.battlesWon++;
        statsController.SaveStats();
    }

    private IEnumerator PlayRewardsRisingAnimation()
    {
        int tempRewards = 0;
        statsController.totalMoneyEarned += winReward;
        statsController.availableMoney += winReward;
        while (tempRewards < winReward)
        {
            rewardsText.text = tempRewards.ToString();
            tempRewards += 1;
            yield return new WaitForSeconds(2.5f / winReward);
        }
        if (tempRewards <= winReward)
        {
            rewardsText.text = winReward.ToString();
        }
    }

    private IEnumerator SecondCounter()
    {
        secondsInBattle = 0;
        while (!battleTimeText.gameObject.activeSelf)
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
