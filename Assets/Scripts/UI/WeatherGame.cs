using UnityEngine;

public class WeatherGame : MonoBehaviour
{
    public GameObject selectWeather;
    public GameObject gameScreen;
    private bool setRain;

    public void SetRainBool(bool value)
    {
        setRain = value;
    }

    public void StartGame()
    {
        selectWeather.SetActive(false);
        gameScreen.SetActive(true);
    }

    public void EndGame(int finalScore)
    {
        selectWeather.SetActive(true);
        gameScreen.SetActive(false);
        print(finalScore);
    }
}
