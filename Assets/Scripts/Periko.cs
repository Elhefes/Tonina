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

    private void OnEnable()
    {
        StartCoroutine(randomAnimTimer());
    }

    private IEnumerator randomAnimTimer()
    {
        while (true)
        {
            if (!textSubject.textIsActive && !inFlight) PickRandomAnim();
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

    private void PickRandomAnim()
    {
        int r = Random.Range(0, 2);
        if (r == 0) animator.SetTrigger("Idle");
        else animator.SetTrigger("Flight1");
    }

    public void EnableInFlight() { inFlight = true; }
    public void DisableInFlight() { inFlight = false; }

    // Code below is copied from Villager.cs -- not good practice but whatever lol

    public void ProcessNextLines()
    {
        if (textSubject.currentIndex < textSubject.textLines.Length)
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
