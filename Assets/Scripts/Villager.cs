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

    public AudioClip[] voiceLines;
    public AudioSource soundPlayer;
    public bool playingVoiceLines;
    private Coroutine voiceCoroutine;

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

    public void ProcessNextLines()
    {
        if (currentIndex < textLines.Length)
        {
            //stop potential previous speech
            soundPlayer.Stop();
            if (voiceCoroutine != null) StopCoroutine(voiceCoroutine);

            string str = textLines[currentIndex];
            int wordCount = CountWords(str);
            voiceCoroutine = StartCoroutine(PlayRandomVoiceLines(wordCount));
            currentIndex++;
        }
    }

    private IEnumerator PlayRandomVoiceLines(int wordCount)
    {
        playingVoiceLines = true;
        for (int i = 0; i < wordCount; i++)
        {
            if (!playingVoiceLines) break;
            int r = Random.Range(0, voiceLines.Length);
            soundPlayer.PlayOneShot(voiceLines[r], PlayerPrefs.GetFloat("soundVolume", 0.5f));
            yield return new WaitForSeconds(voiceLines[r].length * 0.75f);
        }
        soundPlayer.Stop();
        playingVoiceLines = false;
    }

    int CountWords(string str)
    {
        // Split the string by spaces
        string[] words = str.Split(' ');
        // Return the number of words
        return words.Length;
    }
}
