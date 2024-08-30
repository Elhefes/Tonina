using System.Collections;
using UnityEngine;

public class PlaySoundAndDestroy : MonoBehaviour
{
    public AudioSource audioSource;
    private Coroutine destroyerCoroutine;

    private void OnEnable()
    {
        PlayAndDie();
    }

    void PlayAndDie()
    {
        destroyerCoroutine = StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        audioSource.PlayOneShot(audioSource.clip, PlayerPrefs.GetFloat("soundVolume", 0.5f));
        yield return new WaitUntil(() => audioSource.time >= audioSource.clip.length);
        Destroy(gameObject);
    }
}
