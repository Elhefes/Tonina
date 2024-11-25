using System.Collections;
using UnityEngine;

public class PlaySoundAndDestroy : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnEnable()
    {
        PlayAndDie();
    }

    void PlayAndDie()
    {
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        audioSource.volume = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        audioSource.Play();
        yield return new WaitUntil(() => !audioSource.isPlaying);
        Destroy(gameObject);
    }
}
