using System.Collections;
using UnityEngine;

public class BirdSounds : MonoBehaviour
{
    public Transform positionsParent;
    public Transform soundObject;

    public AudioSource audioSource;
    public AudioClip[] birdSoundClips;

    private void Start()
    {
        StartCoroutine("PlaySoundsRandomly");
    }

    public void PlayOneRandomSound()
    {
        soundObject.position = positionsParent.GetChild(Random.Range(0, positionsParent.childCount)).position;
        audioSource.PlayOneShot(birdSoundClips[Random.Range(0, birdSoundClips.Length)], PlayerPrefs.GetFloat("soundVolume", 0.5f));
    }

    IEnumerator PlaySoundsRandomly()
    {
        yield return new WaitForSeconds(10f);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5.6f, 10f));
            PlayOneRandomSound();
        }
    }
}
