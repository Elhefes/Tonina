using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JadeaWinningScreen : MonoBehaviour
{
    public GameObject returnHomeButton;
    public Image progressionBarImage;
    public Image blackProgTextImage;
    public float previousProgressionValue;
    public float currentProgressionValue;

    public TMP_Text bottomTMP;
    public TMP_Text ceilingTMP;
    private int progressionFloorInt;

    public AudioSource audioSource;

    private void OnEnable()
    {
        // Give this object the correct values before enabling

        returnHomeButton.SetActive(false);
        progressionFloorInt = Mathf.FloorToInt(previousProgressionValue);
        bottomTMP.text = progressionFloorInt.ToString();
        ceilingTMP.text = (progressionFloorInt + 1).ToString();
        progressionBarImage.fillAmount = previousProgressionValue - progressionFloorInt;
        blackProgTextImage.fillAmount = previousProgressionValue - progressionFloorInt;
        StartCoroutine(WinningAnimation());
    }

    private IEnumerator WinningAnimation()
    {
        yield return new WaitForSecondsRealtime(0.7f);

        float duration = 1.8f;
        float time = 0f;

        float startValue = previousProgressionValue;
        float endValue = currentProgressionValue;

        int previousFloor = Mathf.FloorToInt(startValue);

        while (time < duration)
        {
            time += Time.deltaTime;

            float currentValue = Mathf.Lerp(startValue, endValue, time / duration);

            int currentFloor = Mathf.FloorToInt(currentValue);

            // Detect level-up
            if (currentFloor > previousFloor)
            {
                audioSource.PlayOneShot(audioSource.clip, PlayerPrefs.GetFloat("soundVolume", 0.5f));
                previousFloor = currentFloor;
            }

            bottomTMP.text = currentFloor.ToString();
            ceilingTMP.text = (currentFloor + 1).ToString();
            progressionBarImage.fillAmount = currentValue - currentFloor;
            blackProgTextImage.fillAmount = currentValue - currentFloor;

            yield return null;
        }

        int finalFloor = Mathf.FloorToInt(endValue);
        bottomTMP.text = finalFloor.ToString();
        ceilingTMP.text = (finalFloor + 1).ToString();
        progressionBarImage.fillAmount = endValue - finalFloor;
        blackProgTextImage.fillAmount = endValue - finalFloor;

        returnHomeButton.SetActive(true);
    }
}
