using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeatherGameResults : MonoBehaviour
{
    public TMP_Text focusText;
    public TMP_Text resultText;
    public WeatherController weatherController;

    public void SetTextsByResult(int finalScore, bool setRain)
    {
        focusText.text = "Your focus was " + finalScore + "%.";
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
                weatherController.SetArtificialWeather(true, 150, 250, 300);
            }
            else if (finalScore < 80)
            {
                resultText.text = "There will be some rain.";
                weatherController.SetArtificialWeather(true, 250, 500, 450);
            }
            else if (finalScore < 88)
            {
                resultText.text = "It will rain as usual.";
                weatherController.SetArtificialWeather(true, 500, 800, 600);
            }
            else if (finalScore < 93)
            {
                resultText.text = "Heavy rain is approaching!";
                weatherController.SetArtificialWeather(true, 800, 1000, 750);
            }
            else
            {
                resultText.text = "Huge rainfall is closing in!";
                weatherController.SetArtificialWeather(true, 1000, 1101, 1200);
            }
        }
        else
        {
            if (finalScore < 70)
            {
                resultText.text = "It will be clear for a bit.";
                weatherController.SetArtificialWeather(false, 0, 0, 300);
            }
            else if (finalScore < 80)
            {
                resultText.text = "It will be clear for some time.";
                weatherController.SetArtificialWeather(false, 0, 0, 450);
            }
            else if (finalScore < 88)
            {
                resultText.text = "Sunny skies are here!";
                weatherController.SetArtificialWeather(false, 0, 0, 600);
            }
            else if (finalScore < 93)
            {
                resultText.text = "Good day sunshine!";
                weatherController.SetArtificialWeather(false, 0, 0, 750);
            }
            else
            {
                resultText.text = "No rain any time soon.";
                weatherController.SetArtificialWeather(false, 0, 0, 1200);
            }
        }
    }

    public void DisableObject() { gameObject.SetActive(false); }
}
