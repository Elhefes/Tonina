using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class WaveController : MonoBehaviour
{
    private List<string> lines;
    private List<string> parsedLines;
    public TextAsset txt;
    public int roundNumber;
    public int friendlyWarriorsAmount;
    private bool isSpawningEnemies;

    public Transform[] spawnPoints;
    private Vector3 housePos;

    private List<Coroutine> coroutines;
    public GameObject enemyPrefab;
    public GameObject friendlyWarriorPrefab;
    private GameObject kingHouse;

    public MusicPlayer musicPlayer;

    void Start()
    {
        kingHouse = GameObject.Find("king_house");
        housePos = kingHouse.transform.position;
        coroutines = new List<Coroutine>();
        print(txt.text);
        lines = new List<string>(txt.text.Split('\n'));
        parsedLines = new List<string>();

        foreach (string line in lines)
        {
            if (line.Length == 0) break;
            parsedLines.Add(line.Substring(4, line.Length - 4));
        }
    }

    public void StartRound(int roundNumber, int battleSongID)
    {
        coroutines.Add(StartCoroutine(ParseRound(parsedLines[roundNumber - 1])));
        musicPlayer.PlayBattleSong(battleSongID);
    }

    public IEnumerator ParseRound(string round)
    {
        List<string> bits = new(round.Split(' '));
        isSpawningEnemies = true;
        bool skipToNextBit = false;
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
        //InvokeRepeating("CheckForEnemies", 1f, 1f);
    }

    void SpawnEnemyOfType(char c, int i)
    {
        if (c.Equals('A')) Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
        return;


        string enemyName = "";
        if (c.Equals('A')) enemyName = "ScarredMummy";
        else if (c.Equals('B')) enemyName = "MintMummy";
        else if (c.Equals('C')) enemyName = "ArmoredMummy";
        else if (c.Equals('D')) enemyName = "Skeleton";
        else if (c.Equals('E')) enemyName = "GoldenSkeleton";
        else if (c.Equals('F')) enemyName = "MummyWagon";
        else if (c.Equals('G')) enemyName = "SkeletonWagon";
        else if (c.Equals('H')) enemyName = "StoneMan";
        else if (c.Equals('I')) enemyName = "Sandman";
        else if (c.Equals('J')) enemyName = "DarkSandman";
        else if (c.Equals('K')) enemyName = "Ibis";
        else if (c.Equals('L')) enemyName = "MummyKing";
        else if (c.Equals('M')) enemyName = "UndeadChariot";
        else if (c.Equals('N')) enemyName = "MiniMummyKing";
        else if (c.Equals('Q')) enemyName = "Anubis";
        else if (c.Equals('R')) enemyName = "Khnum";
        else if (c.Equals('S')) enemyName = "Sobek";
        // gameController.SpawnEnemy(enemyName);
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
}