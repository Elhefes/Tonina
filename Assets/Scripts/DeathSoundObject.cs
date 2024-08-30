using System.Collections;
using UnityEngine;

public class DeathSoundObject : MonoBehaviour
{
    public AudioSource deathAudioSource;
    private Coroutine deathCoroutine;

    private void OnEnable()
    {
        PlayAndDie();
    }

    void PlayAndDie()
    {
        deathCoroutine = StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        deathAudioSource.PlayOneShot(deathAudioSource.clip, PlayerPrefs.GetFloat("soundVolume", 0.5f));
        yield return new WaitUntil(() => deathAudioSource.time >= deathAudioSource.clip.length);
        Destroy(gameObject);
    }
}
