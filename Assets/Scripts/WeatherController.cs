using System.Collections;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public ParticleSystem rainParticleSystem;
    public AudioSource rainSoundSource;
    public AudioLooper audioLooper;
    private float rainSoundFaderValue;
    private bool raining;
    private float rainFogDensity = 0.025f;
    private int currentRainEmissionAmount;
    private int randomRainEmissionAmount;
    private Coroutine rainTransitionCoroutine;
    private Coroutine naturalWeatherCoroutine;
    private Color clearAmbientColor = new Color(0.5411765f, 0.5764706f, 0.5058824f);
    private Color rainAmbientColor = new Color(0.2745098f, 0.3411765f, 0.2745098f);

    private Material skyboxMat;
    private Color clearSkyColor = new Color(0.401f, 0.639f, 1f);
    private Color rainSkyColor = new Color(0.451f, 0.31765f, 0f);
    private Color clearSkyGroundColor = new Color(0.49475f, 0.962f, 0.71f);
    private Color rainSkyGroundColor = new Color(0.388f, 0.388f, 0.388f);
    private float clearAtmosphereThickness = 0.85f;
    private float rainAtmosphereThickness = 1.08f;
    private float clearSkyExposure = 1f;
    private float rainSkyExposure = 0.13f;

    private void Start()
    {
        naturalWeatherCoroutine = StartCoroutine(StartNaturalWeather());
        rainSoundSource.volume = 0f;
        rainSoundSource.clip = audioLooper.trimSilence(rainSoundSource.clip);

        skyboxMat = RenderSettings.skybox;
        SetSunnySkybox();
    }

    private void OnDisable()
    {
        SetSunnySkybox();
    }

    void FixedUpdate()
    {
        // --- Ambient light ---
        Color targetAmbient = raining ? rainAmbientColor : clearAmbientColor;
        RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, targetAmbient, 0.5f * Time.fixedDeltaTime);

        // --- Fog density ---
        float targetFogDensity = raining ? rainFogDensity : 0f;
        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFogDensity, 0.1f * Time.fixedDeltaTime);

        // --- Rain particle emission ---
        var emission = rainParticleSystem.emission;
        emission.rateOverTime = currentRainEmissionAmount;

        // --- Rain sound ---
        float targetRainSound = raining ? (currentRainEmissionAmount / 1100f) : 0f;
        rainSoundFaderValue = Mathf.MoveTowards(rainSoundFaderValue, targetRainSound, 0.0003f);
        rainSoundSource.volume = rainSoundFaderValue * PlayerPrefs.GetFloat("soundVolume", 0.5f);

        // --- Skybox transition ---
        Color targetSkyColor = raining ? rainSkyColor : clearSkyColor;
        Color targetGroundColor = raining ? rainSkyGroundColor : clearSkyGroundColor;
        float targetAtmosphereThickness = raining ? rainAtmosphereThickness : clearAtmosphereThickness;
        float targetExposure = raining ? rainSkyExposure : clearSkyExposure;

        skyboxMat.SetColor("_SkyTint",
            Color.Lerp(skyboxMat.GetColor("_SkyTint"), targetSkyColor, 0.08f * Time.fixedDeltaTime));
        skyboxMat.SetColor("_GroundColor",
            Color.Lerp(skyboxMat.GetColor("_GroundColor"), targetGroundColor, 0.08f * Time.fixedDeltaTime));
        skyboxMat.SetFloat("_AtmosphereThickness",
            Mathf.Lerp(skyboxMat.GetFloat("_AtmosphereThickness"), targetAtmosphereThickness, 0.33f * Time.fixedDeltaTime));
        skyboxMat.SetFloat("_Exposure",
            Mathf.Lerp(skyboxMat.GetFloat("_Exposure"), targetExposure, 0.2f * Time.fixedDeltaTime));
    }

    void SetSunnySkybox()
    {
        skyboxMat.SetColor("_SkyTint", clearSkyColor);
        skyboxMat.SetColor("_GroundColor", clearSkyGroundColor);
        skyboxMat.SetFloat("_AtmosphereThickness", clearAtmosphereThickness);
        skyboxMat.SetFloat("_Exposure", clearSkyExposure);
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
