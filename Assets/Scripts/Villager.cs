using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : CreatureMovement
{
    private Vector3 startingPosition;
    private Coroutine freeMovementCoroutine;
    public float freeSpaceX;
    public float freeSpaceZ;
    public float minWaitTime;
    public float maxWaitTime;

    public bool talking;
    public string[] textLines;
    public int currentIndex;

    private GameObject player;

    void Awake()
    {
        startingPosition = transform.position;
        freeMovementCoroutine = StartCoroutine(MoveRandomly());
    }

    private void Update()
    {
        base.Update();
        if (talking && player != null)
        {
            // Calculate the direction to the destination
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // Ignore the y-axis to prevent tilting
            direction.y = 0f;

            // Rotate towards the destination
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * agent.angularSpeed * 0.25f);
            }
        }
    }

    public void TalkToPlayer(GameObject p)
    {
        player = p;
        agent.destination = transform.position;
    }

    private IEnumerator MoveRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            if (!talking) agent.destination = new Vector3(startingPosition.x + Random.Range(-freeSpaceX, freeSpaceX), startingPosition.y, startingPosition.z + Random.Range(-freeSpaceZ, freeSpaceZ));
        }
    }

    public void ProcessNextString()
    {
        if (currentIndex < textLines.Length)
        {
            string str = textLines[currentIndex];
            int wordCount = CountWords(str);
            //Debug.Log($"String {currentIndex + 1}: \"{str}\", Word Count: {wordCount}");

            int[] randomNumbers = GenerateRandomNumbers(wordCount);
            //Debug.Log("Random Numbers:");
            //foreach (var num in randomNumbers)
            //{
            //    Debug.Log(num + " ");
            //}
            //Debug.Log("\n");

            currentIndex++;
        }
        else
        {
            //Debug.Log("No more strings to process.");
        }
    }

    int CountWords(string str)
    {
        // Split the string by spaces
        string[] words = str.Split(' ');
        // Return the number of words
        return words.Length;
    }

    int[] GenerateRandomNumbers(int max)
    {
        System.Random rand = new System.Random();
        // Create an array to store the numbers
        List<int> numbers = new List<int>();
        // Add numbers from 0 to max-1
        for (int i = 0; i < max; i++)
        {
            numbers.Add(i);
        }

        int[] randomNumbers = new int[max];
        // Shuffle the numbers
        for (int i = 0; i < max; i++)
        {
            int index = rand.Next(0, numbers.Count);
            randomNumbers[i] = numbers[index];
            numbers.RemoveAt(index);
        }

        return randomNumbers;
    }
}
