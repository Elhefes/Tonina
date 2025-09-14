using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;

public class WaveController : MonoBehaviour
{
    public ThreatLevels threatLevels;
    private ThreatLevels.ThreatLevel threatLevel;
    private int currentRoundNumber;
    private int friendlyWarriorsAmount;
    private bool isSpawningEnemies;
    private int secondsInBattle;
    public bool battleIsLost;

    public Transform[] spawnPoints;

    public StatsController statsController;

    private List<Coroutine> coroutines;
    private Coroutine rewardsRisingCoroutine;
    public GameObject overworldOptionsButton;
    public GameObject battleUI;
    public GameObject battleWinningScreen;
    public TMP_Text threatLevelText;
    public TMP_Text battleTimeText;
    public TMP_Text rewardsText;
    private GameObject kingHouse;

    public MusicPlayer musicPlayer;

    public GameObject friendlyWarriorPrefab;

    [Header("Enemies")]
    public GameObject clubber;
    public GameObject runner;
    public GameObject spearWarrior;
    public GameObject axeWarrior;

    public ObjectPooler pooler;

    void Start()
    {
        kingHouse = GameObject.Find("king_house");
        coroutines = new List<Coroutine>();
        pooler = ObjectPooler.Instance;
    }

    public void StartRound(int roundNumber, int battleSongID)
    {
        battleIsLost = false;
        currentRoundNumber = roundNumber;
        threatLevel = threatLevels.GetThreatLevel(roundNumber - 1);
        friendlyWarriorsAmount = threatLevel.friendlyWarriorsAmount;
        coroutines.Add(StartCoroutine(ParseRound(threatLevel.wave)));
        musicPlayer.PlayBattleSong(battleSongID);
        EnableBattleUI();
        StartCoroutine(SecondCounter());
    }

    void EnableBattleUI()
    {
        overworldOptionsButton.SetActive(false);
        battleUI.SetActive(true);
    }

    void DisableBattleUI()
    {
        battleUI.SetActive(false);
        overworldOptionsButton.SetActive(true);
    }

    public IEnumerator ParseRound(string round)
    {
        List<string> bits = new(round.Split(' '));
        isSpawningEnemies = true;
        int index;

        if (friendlyWarriorsAmount > 0) SpawnFriendlies(friendlyWarriorsAmount);

        foreach (string bit in bits)
        {
            if (bit.StartsWith("P"))
            {
                int delay = Int32.Parse(bit.Substring(1, bit.Length - 1));
                float spawnHaltDelay = (float)delay / 10;
                yield return new WaitForSeconds(spawnHaltDelay);
            }
            else
            {
                bool skipToNextBit = false;
                var splittedBit = bit.Split('-');
                Int32.TryParse(splittedBit[1], out index);
                string bitStart = splittedBit[0];
            
                if (bitStart.StartsWith("X"))
                {
                    skipToNextBit = true;
                }
                int enemyCount = 0;
                string intConstructor = "";
                char enemy = 'a';

                foreach (char c in bitStart)
                {
                    if (Char.IsDigit(c))
                    {
                        intConstructor += c;
                    }
                    else if (Char.IsLetter(c))
                    {
                        enemy = c;
                        Int32.TryParse(intConstructor, out enemyCount);
                        intConstructor = "";
                    }
                }

                int time = Int32.Parse(intConstructor);
                float spawnDuration = (float)time / 10 / (float)enemyCount;

                int spawned = 0;
                while (spawned < enemyCount)
                {
                    SpawnEnemyOfType(enemy, index);
                    spawned++;
                    if (!skipToNextBit) yield return new WaitForSeconds(spawnDuration);
                }
            }
        }
        InvokeRepeating("CheckForEnemies", 1f, 1f);
    }

    void SpawnEnemyOfType(char c, int i)
    {
        string enemyType = "Clubber";
        if (c.Equals('B')) enemyType = "Runner";
        if (c.Equals('C')) enemyType = "SpearWarrior";
        if (c.Equals('D')) enemyType = "AxeWarrior";

        Vector3 spawn = spawnPoints[i].position + new Vector3(UnityEngine.Random.Range(-10f, 10f), 0f, 0f);
        Enemy enemy = ObjectPooler.Instance.SpawnEnemyFromPool(enemyType, spawn, spawnPoints[i].rotation);
        enemy.ResetEnemyAttributes();
    }

    void CheckForEnemies()
    {
        if (battleIsLost) return;
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            isSpawningEnemies = false;
            CancelInvoke("CheckForEnemies");
            WinBattle();
        }
    }

    public void LoseBattle()
    {
        isSpawningEnemies = false;
        CancelInvoke("CheckForEnemies");
        StopAllCoroutines();
        DisableBattleUI();
        if (secondsInBattle < 16) statsController.battlesForfeited++;
        else statsController.battlesLost++;
        statsController.SaveStats();
    }

    void WinBattle()
    {
        battleWinningScreen.SetActive(true);
        threatLevelText.text = currentRoundNumber.ToString();
        battleTimeText.text = GetBattleTimerString(secondsInBattle);
        StopCoroutine(SecondCounter());
        rewardsRisingCoroutine = StartCoroutine(PlayRewardsRisingAnimation());
        DisableBattleUI();
        statsController.battlesWon++;
        statsController.SaveStats();
    }

    int GetTotalRewards()
    {
        if (secondsInBattle <= threatLevel.rewardTimerMin) return threatLevel.maxReward;
        else if (secondsInBattle >= threatLevel.rewardTimerMax) return threatLevel.minReward;
        return Mathf.RoundToInt(threatLevel.maxReward - (threatLevel.maxReward - threatLevel.minReward) * 
            ((float)secondsInBattle - threatLevel.rewardTimerMin) / (threatLevel.rewardTimerMax - threatLevel.rewardTimerMin));
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

    void SpawnFriendlies(int friendlyWarriorsAmount)
    {
        float xOffset = -11f;
        float zOffset = -11f;
        float xStep = 1.5f;
        float zStep = 1.5f;

        int pairs = friendlyWarriorsAmount / 2;
        bool isOdd = friendlyWarriorsAmount % 2 != 0;

        for (int i = 0; i < pairs; i++)
        {
            SpawnFriendlyPair(new Vector3(kingHouse.transform.position.x + xOffset, kingHouse.transform.position.y, kingHouse.transform.position.z + zOffset));

            if ((i + 1) % 3 == 0)
            {
                zOffset += zStep;
                xOffset = -11f; // Reset xOffset for the next row
            }
            else xOffset -= xStep; // Increment xOffset using xStep
            
        }

        if (isOdd)
        {
            // Spawn a single friendly warrior to the negative x side
            Vector3 spawn = new Vector3(kingHouse.transform.position.x + xOffset, kingHouse.transform.position.y, kingHouse.transform.position.z + zOffset);
            ToninaWarrior friendly = ObjectPooler.Instance.SpawnFriendlyFromPool("ToninaWarrior", spawn, kingHouse.transform.rotation);
            friendly.ResetFriendlyAttributes();
        }
    }

    void SpawnFriendlyPair(Vector3 spawnPosition)
    {
        ToninaWarrior friendly1 = ObjectPooler.Instance.SpawnFriendlyFromPool("ToninaWarrior", spawnPosition, kingHouse.transform.rotation);
        friendly1.ResetFriendlyAttributes();
        Vector3 spawn2 = new Vector3((spawnPosition.x * -1f) - 2f, spawnPosition.y, spawnPosition.z);
        ToninaWarrior friendly2 = ObjectPooler.Instance.SpawnFriendlyFromPool("ToninaWarrior", spawn2, kingHouse.transform.rotation);
        friendly2.ResetFriendlyAttributes();
    }

    IEnumerator SecondCounter()
    {
        secondsInBattle = 0;
        while (!battleWinningScreen.activeSelf)
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