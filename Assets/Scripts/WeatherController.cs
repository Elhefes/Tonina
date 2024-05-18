using System.Collections;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public Weather weather;
    public ParticleSystem rainParticleSystem;
    private bool raining;
    private float rainFogDensity = 0.025f;
    private int currentRainEmissionAmount;
    private int randomRainEmissionAmount;
    private Coroutine rainTransitionCoroutine;
    private Color clearAmbientColor = new Color(0.5411765f, 0.5764706f, 0.5058824f);
    private Color rainAmbientColor = new Color(0.2745098f, 0.3411765f, 0.2745098f);

    void FixedUpdate()
    {
        if (weather == Weather.Rain && !raining) StartRain();
        if (weather == Weather.Clear && raining) StopRain();
        if (raining)
        {
            if (Mathf.Clamp(RenderSettings.ambientLight.r, rainAmbientColor.r, clearAmbientColor.r) > rainAmbientColor.r)
                RenderSettings.ambientLight -= new Color(0.0003f, 0f, 0f);
            if (Mathf.Clamp(RenderSettings.ambientLight.g, rainAmbientColor.g, clearAmbientColor.g) > rainAmbientColor.g)
                RenderSettings.ambientLight -= new Color(0f, 0.0003f, 0f);
            if (Mathf.Clamp(RenderSettings.ambientLight.b, rainAmbientColor.b, clearAmbientColor.b) > rainAmbientColor.b)
                RenderSettings.ambientLight -= new Color(0f, 0f, 0.0003f);

            if (Mathf.Clamp(RenderSettings.fogDensity, 0f, rainFogDensity) < rainFogDensity) RenderSettings.fogDensity += 0.00001f;

            var emission = rainParticleSystem.emission;
            emission.rateOverTime = currentRainEmissionAmount;
        }
        else
        {
            if (Mathf.Clamp(RenderSettings.ambientLight.r, rainAmbientColor.r, clearAmbientColor.r) < clearAmbientColor.r)
                RenderSettings.ambientLight += new Color(0.0003f, 0f, 0f);
            if (Mathf.Clamp(RenderSettings.ambientLight.g, rainAmbientColor.g, clearAmbientColor.g) < clearAmbientColor.g)
                RenderSettings.ambientLight += new Color(0f, 0.0003f, 0f);
            if (Mathf.Clamp(RenderSettings.ambientLight.b, rainAmbientColor.b, clearAmbientColor.b) < clearAmbientColor.b)
                RenderSettings.ambientLight += new Color(0f, 0f, 0.0003f);

            if (RenderSettings.fogDensity > 0) RenderSettings.fogDensity -= 0.00001f;

            if (currentRainEmissionAmount > 0)
            {
                var emission = rainParticleSystem.emission;
                emission.rateOverTime = currentRainEmissionAmount;
            }
        }
    }

    void StartRain()
    {
        raining = true;
        rainParticleSystem.Play();
        randomRainEmissionAmount = Random.Range(150, 1100);
        rainTransitionCoroutine = StartCoroutine(RainBuildUp());
    }

    void StopRain()
    {
        raining = false;
        StopCoroutine(RainBuildUp());
        rainTransitionCoroutine = StartCoroutine(FadeRainAway());
    }

    public enum Weather
    {
        Clear,
        Rain,
    }

    private IEnumerator RainBuildUp()
    {
        while ((currentRainEmissionAmount < randomRainEmissionAmount) && raining)
        {
            if (currentRainEmissionAmount + 25 > randomRainEmissionAmount)
            {
                currentRainEmissionAmount += randomRainEmissionAmount - currentRainEmissionAmount;
            }
            else currentRainEmissionAmount += 25;
            yield return new WaitForSeconds(1.8f);
        }
    }

    private IEnumerator FadeRainAway()
    {
        while (currentRainEmissionAmount > 0 && !raining)
        {
            if (currentRainEmissionAmount - 44 < 0)
            {
                currentRainEmissionAmount -= currentRainEmissionAmount;
            }
            else currentRainEmissionAmount -= 44;
            yield return new WaitForSeconds(1.8f);
        }
        rainParticleSystem.Stop();
    }
}
