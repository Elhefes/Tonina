using System.Collections;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public ParticleSystem rainParticleSystem;
    public AudioSource rainSoundSource;
    private float rainSoundFaderValue;
    private bool raining;
    private float rainFogDensity = 0.025f;
    private int currentRainEmissionAmount;
    private int randomRainEmissionAmount;
    private Coroutine rainTransitionCoroutine;
    private Coroutine naturalWeatherCoroutine;
    private Color clearAmbientColor = new Color(0.5411765f, 0.5764706f, 0.5058824f);
    private Color rainAmbientColor = new Color(0.2745098f, 0.3411765f, 0.2745098f);

    private void Start()
    {
        naturalWeatherCoroutine = StartCoroutine(StartNaturalWeather());
        rainSoundSource.volume = 0f;
    }

    void FixedUpdate()
    {
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

            if (rainSoundFaderValue < (currentRainEmissionAmount / 1100f)) rainSoundFaderValue += 0.00012f;
            else if (rainSoundFaderValue > (currentRainEmissionAmount / 1100f) + 0.00012f) rainSoundFaderValue -= 0.0003f;

            rainSoundSource.volume = rainSoundFaderValue * PlayerPrefs.GetFloat("soundVolume", 0.5f);
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

            if (rainSoundFaderValue > 0f)
            {
                if (rainSoundFaderValue > (currentRainEmissionAmount / 1100f)) rainSoundFaderValue -= 0.0003f;
                rainSoundSource.volume = rainSoundFaderValue * PlayerPrefs.GetFloat("soundVolume", 0.5f);
            }
        }
    }

    void StartRain(int randomMin, int randomMax)
    {
        raining = true;
        StopCoroutine(FadeRainAway());
        rainParticleSystem.Play();
        randomRainEmissionAmount = Random.Range(randomMin, randomMax);
        rainTransitionCoroutine = StartCoroutine(RainBuildUp());
        rainSoundSource.Play();
    }

    void StopRain()
    {
        raining = false;
        StopCoroutine(RainBuildUp());
        rainTransitionCoroutine = StartCoroutine(FadeRainAway());
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

        // If heavy rainfall switches to lighter rainfall
        while ((currentRainEmissionAmount > randomRainEmissionAmount) && raining)
        {
            if (currentRainEmissionAmount - 25 < randomRainEmissionAmount)
            {
                currentRainEmissionAmount = randomRainEmissionAmount;
            }
            else currentRainEmissionAmount -= 25;
            yield return new WaitForSeconds(1.8f);
        }
    }

    private IEnumerator FadeRainAway()
    {
        while (currentRainEmissionAmount > 0 && !raining)
        {
            if (currentRainEmissionAmount - 44 < 0)
            {
                currentRainEmissionAmount = 0;
            }
            else currentRainEmissionAmount -= 44;
            yield return new WaitForSeconds(1.8f);
        }
        if (!raining)
        {
            rainParticleSystem.Stop();
            rainSoundSource.Stop();
            rainSoundSource.volume = 0f;
            rainSoundFaderValue = 0f;
        }
    }

    private IEnumerator StartNaturalWeather()
    {
        yield return new WaitForSeconds(30);

        // Stop artificial rain
        if (raining)
        {
            yield return new WaitForSeconds(Random.Range(0, 121));
            StopRain();
            yield return new WaitForSeconds(30);
        }

        while (true)
        {
            if (Random.Range(1, 9) == 1)
            {
                // Natural rain emission is between 150 and 1100
                StartRain(150, 1101);
                int rainLength = Random.Range(240, 721);
                yield return new WaitForSeconds(rainLength);
                StopRain();
                // Guarantee 4 minutes of clear weather after a long rain
                if (rainLength > 480) yield return new WaitForSeconds(240);
            }
            else yield return new WaitForSeconds(60);
        }
    }

    public void SetArtificialWeather(bool setRain, int randomMin, int randomMax, int effectLength)
    {
        StopAllCoroutines();
        if (setRain)
        {
            StartRain(randomMin, randomMax);
        }
        else
        {
            StopRain();
        }
        rainTransitionCoroutine = StartCoroutine(KeepArtificialWeather(effectLength));
    }

    private IEnumerator KeepArtificialWeather(int effectLength)
    {
        yield return new WaitForSeconds(effectLength - 30);
        StartCoroutine(StartNaturalWeather());
    }
}
