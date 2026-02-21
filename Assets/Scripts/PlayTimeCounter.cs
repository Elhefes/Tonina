using System;
using System.Collections;
using UnityEngine;

public class PlayTimeCounter : MonoBehaviour
{
    // Would it be better to have different time counters for each save file?

    int secondsPlayed;

    private void Start()
    {
        secondsPlayed = PlayerPrefs.GetInt("secondsPlayed", 0);
        StartCoroutine(SecondCounter());
    }

    IEnumerator SecondCounter()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            secondsPlayed++;
            PlayerPrefs.SetInt("secondsPlayed", secondsPlayed);
        }
    }
}
