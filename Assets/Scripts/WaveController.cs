using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class WaveController : MonoBehaviour
{
    private List<string> lines;
    private List<string> parsedLines;
    public TextAsset txt;
    public int roundNumber = 1;
    private bool isSpawningEnemies;

    public Transform[] spawnPoints;

    private List<Coroutine> coroutines;
    public GameObject enemyPrefab;

    void Start()
    {
        coroutines = new List<Coroutine>();
        print(txt.text);
        lines = new List<string>(txt.text.Split('\n'));
        parsedLines = new List<string>();

        foreach (string line in lines)
        {
            if (line.Length == 0) break;
            parsedLines.Add(line.Substring(4, line.Length - 4));
        }
        StartRound();
    }

    public void StartRound(string roundDefinition = null)
    {
        coroutines.Add(StartCoroutine(ParseRound(parsedLines[roundNumber - 1])));
        if (roundNumber < 100) roundNumber++;
    }

    public IEnumerator ParseRound(string round)
    {
        List<string> bits = new(round.Split(' '));
        isSpawningEnemies = true;
        bool skipToNextBit;
        int index;
        
        foreach (string bit in bits)
        {
            if (bit.StartsWith("P"))
            {
                int delay = Int32.Parse(bit.Substring(1, bit.Length - 1));
                float spawnHaltDelay = (float)delay / 10;
                yield return new WaitForSeconds(spawnHaltDelay);
            }
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
                yield return new WaitForSeconds(spawnDuration);
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

}