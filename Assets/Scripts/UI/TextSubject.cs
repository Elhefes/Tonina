using UnityEngine;

public class TextSubject : MonoBehaviour
{
    public bool textIsActive;

    public bool uiPopUpAfterLastLine;
    public GameObject[] popUpElements;

    public string[] textLines;
    public int currentIndex;

    [HideInInspector] public Villager villager;
    [HideInInspector] public Periko periko;

    private void Awake()
    {
        villager = GetComponent<Villager>();
        periko = GetComponent<Periko>();
    }

    public bool DialogueCompleted()
    {
        if (periko != null)
            return currentIndex >= periko.randomVoiceLinesLength;

        return currentIndex >= textLines.Length;
    }

    public void ProcessNextLine()
    {
        if (villager != null)
        {
            villager.ProcessNextLines();
        }
        else if (periko != null)
        {
            periko.ProcessNextLines();
        }

        currentIndex++;
    }

    public void StopSpeaking()
    {
        if (villager != null)
        {
            villager.playingVoiceLines = false;
        }

        if (periko != null)
        {
            periko.playingVoiceLines = false;
        }
    }

    public void ShowPopup()
    {
        if (!uiPopUpAfterLastLine) return;
        if (popUpElements == null) return;

        foreach (GameObject obj in popUpElements)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}