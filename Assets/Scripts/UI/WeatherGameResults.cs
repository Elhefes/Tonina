using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeatherGameResults : MonoBehaviour
{
    public TMP_Text focusText;
    public TMP_Text resultText;

    public void SetTextsByResult(int finalScore, bool setRain)
    {
        focusText.text = "Your focus was " + finalScore + "%";
        if (finalScore < 50)
        {
            resultText.text = "It wasn't enough...";
            return;
        }

        if (setRain)
        {
            if (finalScore < 70)
            {
                resultText.text = "It will rain a little.";
            }
            else if (finalScore < 80)
            {
                resultText.text = "There will be some rain.";
            }
            else if (finalScore < 88)
            {
                resultText.text = "It will rain as usual.";
            }
            else if (finalScore < 93)
            {
                resultText.text = "Heavy rain is approaching!";
            }
            else
            {
                resultText.text = "Huge rainfall is closing in!";
            }
        }
        else
        {
            if (finalScore < 70)
            {
                resultText.text = "It will be clear for a bit.";
            }
            else if (finalScore < 80)
            {
                resultText.text = "It will be clear for some time.";
            }
            else if (finalScore < 88)
            {
                resultText.text = "Sunny skies are here!";
            }
            else if (finalScore < 93)
            {
                resultText.text = "Good day sunshine!";
            }
            else
            {
                resultText.text = "No rain any time soon.";
            }
        }
    }

    public void DisableObject() { gameObject.SetActive(false); }
}
