using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public Weather weather;
    public ParticleSystem rainParticleSystem;
    private bool raining;
    private float rainFogDensity = 0.025f;
    private Color clearAmbientColor = new Color(0.5411765f, 0.5764706f, 0.5058824f);
    private Color rainAmbientColor = new Color(0.2745098f, 0.3411765f, 0.2745098f);

    void Update()
    {
        if (weather == Weather.Rain && !raining) StartRain();
        if (weather == Weather.Clear && raining) StopRain();
    }

    void StartRain()
    {
        raining = true;
        RenderSettings.ambientLight = rainAmbientColor;
        RenderSettings.fogDensity = rainFogDensity;
        rainParticleSystem.Play();
    }

    void StopRain()
    {
        raining = false;
        RenderSettings.ambientLight = clearAmbientColor;
        RenderSettings.fogDensity = 0f;
        rainParticleSystem.Stop();
    }

    public enum Weather
    {
        Clear,
        Rain,
    }
}
