using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinningScreen : MonoBehaviour
{
    public GameObject returnHomeButton;
    public Image progressionBarImage;
    public float previousProgressionValue;
    public float currentProgressionValue;

    public TMP_Text bottomTMP;
    public TMP_Text ceilingTMP;
    private int progressionFloorInt;

    private void OnEnable()
    {
        // Give this object the correct values before enabling

        returnHomeButton.SetActive(false);
        progressionFloorInt = Mathf.FloorToInt(previousProgressionValue);
        bottomTMP.text = progressionFloorInt.ToString();
        ceilingTMP.text = (progressionFloorInt + 1).ToString();
        progressionBarImage.fillAmount = previousProgressionValue - progressionFloorInt;
        StartCoroutine(WinningAnimation());
    }

    private IEnumerator WinningAnimation()
    {
        yield return new WaitForSecondsRealtime(0.7f);

        float duration = 1.8f;
        float time = 0f;

        float startValue = previousProgressionValue;
        float endValue = currentProgressionValue;

        while (time < duration)
        {
            time += Time.deltaTime;

            float currentValue = Mathf.Lerp(startValue, endValue, time / duration);

            int floor = Mathf.FloorToInt(currentValue);

            bottomTMP.text = floor.ToString();
            ceilingTMP.text = (floor + 1).ToString();
            progressionBarImage.fillAmount = currentValue - floor;

            yield return null;
        }

        int finalFloor = Mathf.FloorToInt(endValue);
        bottomTMP.text = finalFloor.ToString();
        ceilingTMP.text = (finalFloor + 1).ToString();
        progressionBarImage.fillAmount = endValue - finalFloor;

        returnHomeButton.SetActive(true);
    }
}
