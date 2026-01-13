using System.Collections;
using UnityEngine;
using TMPro;

public class IntroEndTexts : MonoBehaviour
{
    public TMP_Text welcomeTMP;
    public TMP_Text jadeTMP;
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

        fullText = jadeTMP.text;
        jadeTMP.text = "";
        jadeTMP.gameObject.SetActive(true);

        for (int i = 0; i < fullText.Length + 1; i++)
        {
            currentText = fullText.Substring(0, i);
            yield return new WaitForSeconds(0.15f);
            jadeTMP.text = currentText;
        }

        yield return new WaitForSeconds(3f);

        SceneChangingManager.Instance.LoadScene("Jadea");
    }
}
