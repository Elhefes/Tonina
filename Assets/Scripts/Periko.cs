using System.Collections;
using UnityEngine;

public class Periko : MonoBehaviour
{
    public Animator animator;
    public bool inFlight;

    public TextSubject textSubject;

    public AudioClip[] voiceLines;
    public AudioSource soundPlayer;
    public bool playingVoiceLines;
    private Coroutine voiceCoroutine;

    public int randomVoiceLinesLength;

    private void OnEnable()
    {
        inFlight = false;
        animator.SetTrigger("Idle");

        StartCoroutine(randomAnimTimer());
    }

    private IEnumerator randomAnimTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 15f));
            if (!textSubject.textIsActive && !inFlight) PickRandomAnim();
        }
    }

    private void PickRandomAnim()
    {
        int r = Random.Range(0, 2);
        if (r == 0)
        {
            inFlight = false;
            animator.SetTrigger("Idle");
        }
        else
        {
            inFlight = true;
            animator.SetTrigger("Flight1");
        }
    }

    public void EnableInFlight() { inFlight = true; }
    public void DisableInFlight() { inFlight = false; }

    // Code below is mostly copied from Villager.cs -- not good practice but whatever lol

    public void ProcessNextLines()
    {
        if (textSubject.currentIndex == 0)
        {
            // Pick 1 to 4 random lines
            randomVoiceLinesLength = Random.Range(1, 5);
            ReshuffleArray(textSubject.textLines);
        }

        if (textSubject.currentIndex < randomVoiceLinesLength)
        {
            //stop potential previous speech
            soundPlayer.Stop();
            if (voiceCoroutine != null) StopCoroutine(voiceCoroutine);

            string str = textSubject.textLines[textSubject.currentIndex];
            int wordCount = CountWords(str);
            voiceCoroutine = StartCoroutine(PlayRandomVoiceLines(wordCount));
            textSubject.currentIndex++;
        }
    }

    void ReshuffleArray(string[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++)
        {
            string tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
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
