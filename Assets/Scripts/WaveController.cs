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
    private Vector3 housePos;

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

    void Start()
    {
        kingHouse = GameObject.Find("king_house");
        housePos = kingHouse.transform.position;
        coroutines = new List<Coroutine>();
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
        if (c.Equals('A')) Instantiate(clubber, spawnPoints[i].position, spawnPoints[i].rotation);
        else if (c.Equals('B')) Instantiate(runner, spawnPoints[i].position, spawnPoints[i].rotation);
        else if (c.Equals('C')) Instantiate(spearWarrior, spawnPoints[i].position, spawnPoints[i].rotation);
        else if (c.Equals('D')) Instantiate(axeWarrior, spawnPoints[i].position, spawnPoints[i].rotation);
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
        statsController.battlesLost++;
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
        if (secondsInBattle <= threatLevel.timeLimitForBonusReward) return threatLevel.baseReward + threatLevel.timeBonusReward;
        return threatLevel.baseReward;
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
            SpawnFriendlyPair(new Vector3(housePos.x + xOffset, housePos.y, housePos.z + zOffset));

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
            Instantiate(friendlyWarriorPrefab, new Vector3(housePos.x + xOffset, housePos.y, housePos.z + zOffset), kingHouse.transform.rotation);
        }
    }

    void SpawnFriendlyPair(Vector3 spawnPosition)
    {
        Instantiate(friendlyWarriorPrefab, spawnPosition, kingHouse.transform.rotation);
        Instantiate(friendlyWarriorPrefab, new Vector3((spawnPosition.x * -1f) - 2f, spawnPosition.y, spawnPosition.z), kingHouse.transform.rotation);
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