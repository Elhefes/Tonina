using System.Collections;
using UnityEngine;

public class PlaySoundAndDestroy : MonoBehaviour
{
    public AudioSource audioSource;
    private GameObject parentObject;

    private SmallStone smallStone;
    private Arrow arrow;

    private void OnEnable()
    {
        if (transform.parent.gameObject != null) parentObject = transform.parent.gameObject;
        PlayAndDie();
    }

    void PlayAndDie()
    {
        StartCoroutine(DeathCoroutine());
    }

    void ReturnToParent()
    {
        transform.SetParent(parentObject.transform, true);

        smallStone = parentObject.GetComponent<SmallStone>();
        if (smallStone != null)
        {
            smallStone.hitSoundObject = gameObject;
            return;
        }
        arrow = parentObject.GetComponent<Arrow>();
        if (arrow != null)
        {
            arrow.hitSoundObject = gameObject;
        }
    }

    IEnumerator DeathCoroutine()
    {
        audioSource.volume = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        audioSource.Play();
        yield return new WaitUntil(() => !audioSource.isPlaying);
        if (parentObject != null)
        {
            gameObject.SetActive(false);
            ReturnToParent();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
