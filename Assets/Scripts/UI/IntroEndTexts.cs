using System.Collections;
using UnityEngine;
using TMPro;

public class IntroEndTexts : MonoBehaviour
{
    public TMP_Text welcomeTMP;
    public TMP_Text revolutionTMP;
    private string fullText;
    private string currentText = "";

    private void OnEnable()
    {
        StartCoroutine(TypeEndTexts());
    }

    private IEnumerator TypeEndTexts()
    {
        fullText = welcomeTMP.text;
        welcomeTMP.text = "";

        for (int i = 0; i < fullText.Length + 1; i++)
        {
            currentText = fullText.Substring(0, i);
            yield return new WaitForSeconds(0.12f);
            welcomeTMP.text = currentText;
        }

        yield return new WaitForSeconds(0.5f);

        fullText = revolutionTMP.text;
        revolutionTMP.text = "";
        revolutionTMP.gameObject.SetActive(true);

        for (int i = 0; i < fullText.Length + 1; i++)
        {
            currentText = fullText.Substring(0, i);
            yield return new WaitForSeconds(0.18f);
            revolutionTMP.text = currentText;
        }

        yield return new WaitForSeconds(3f);

        SceneChangingManager.Instance.LoadScene("Tonina");
    }
}
