using System.Collections;
using TMPro;
using UnityEngine;

public class AttackModeController : MonoBehaviour
{
    public UI_Controller UIController;
    public GameObject playerObject;
    public GameObject battleUI;
    public AttackWinScreen attackWinScreen;
    public StatsController statsController;
    public TMP_Text battleTimeText;
    public TMP_Text rewardsText;
    private int secondsInBattle;

    public void StartAttack()
    {
        InvokeRepeating("CheckForEnemies", 1f, 1f);
        StartCoroutine(SecondCounter());
    }

    private void CheckForEnemies()
    {
        if (!playerObject.activeSelf) return;
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

        if (attackWinScreen != null) attackWinScreen.gameObject.SetActive(true);

        statsController.battlesWon++;
        statsController.SaveStats();
    }

    private int GetTotalRewards()
    {
        return attackWinScreen.winReward;
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
        while (!attackWinScreen.gameObject.activeSelf)
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
