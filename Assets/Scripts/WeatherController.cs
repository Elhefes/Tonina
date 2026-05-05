using System.Collections;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public ParticleSystem rainParticleSystem;
    public AudioSource rainSoundSource;
    public AudioLooper audioLooper;
    private float rainSoundFaderValue;
    private bool raining;
    private float rainFogDensity;
    private int currentRainEmissionAmount;
    private int randomRainEmissionAmount;
    private Coroutine rainTransitionCoroutine;
    private Coroutine naturalWeatherCoroutine;
    private Color clearAmbientColor;
    private Color rainAmbientColor;

    private Material skyboxMat;
    private Color clearSkyColor;
    private Color rainSkyColor;
    private Color clearSkyGroundColor;
    private Color rainSkyGroundColor;
    private float clearAtmosphereThickness;
    private float rainAtmosphereThickness;
    private float clearSkyExposure;
    private float rainSkyExposure;

    // --- Wind / gust system ---
    private Vector2 currentWindX; // (start, end)
    private Vector2 currentWindZ;

    private Vector2 targetWindX;
    private Vector2 targetWindZ;

    private float windLerpSpeed;
    private Coroutine windCoroutine;

    private void Start()
    {
        rainFogDensity = 0.025f;
        clearAmbientColor = new Color(0.5411765f, 0.5764706f, 0.5058824f);
        rainAmbientColor = new Color(0.2745098f, 0.3411765f, 0.2745098f);

        clearSkyColor = new Color(0.401f, 0.639f, 1f);
        rainSkyColor = new Color(0.451f, 0.31765f, 0f);
        clearSkyGroundColor = new Color(0.49475f, 0.962f, 0.71f);
        rainSkyGroundColor = new Color(0.388f, 0.388f, 0.388f);
        clearAtmosphereThickness = 0.85f;
        rainAtmosphereThickness = 1.08f;
        clearSkyExposure = 1f;
        rainSkyExposure = 0.13f;
        windLerpSpeed = 1.5f;

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

        // --- Wind smoothing ---
        currentWindX = Vector2.Lerp(currentWindX, targetWindX, windLerpSpeed * Time.fixedDeltaTime);
        currentWindZ = Vector2.Lerp(currentWindZ, targetWindZ, windLerpSpeed * Time.fixedDeltaTime);

        // Apply to particle system
        var vel = rainParticleSystem.velocityOverLifetime;

        // X axis
        var x = vel.x;
        x.mode = ParticleSystemCurveMode.TwoConstants;
        x.constantMin = currentWindX.x;
        x.constantMax = currentWindX.y;
        vel.x = x;

        // Z axis
        var z = vel.z;
        z.mode = ParticleSystemCurveMode.TwoConstants;
        z.constantMin = currentWindZ.x;
        z.constantMax = currentWindZ.y;
        vel.z = z;
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

        // START WIND
        if (windCoroutine != null) StopCoroutine(windCoroutine);
        windCoroutine = StartCoroutine(WindGustRoutine());
    }

    void StopRain()
    {
        raining = false;
        StopCoroutine(RainBuildUp());
        rainTransitionCoroutine = StartCoroutine(FadeRainAway());

        // STOP WIND
        if (windCoroutine != null) StopCoroutine(windCoroutine);

        // Reset wind smoothly back to zero
        targetWindX = Vector2.zero;
        targetWindZ = Vector2.zero;
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

    private IEnumerator WindGustRoutine()
    {
        while (raining)
        {
            // --- Bias toward 0 using squared distribution ---
            float xStart = Random.Range(-1f, 1f);
            float zStart = Random.Range(-1f, 1f);

            // Square to bias toward 0, then scale to [-10, 10]
            xStart = Mathf.Sign(xStart) * xStart * xStart * 10f;
            zStart = Mathf.Sign(zStart) * zStart * zStart * 10f;

            // End values are half
            targetWindX = new Vector2(xStart * 0.5f, xStart);
            targetWindZ = new Vector2(zStart * 0.5f, zStart);

            // --- Intensity affects duration ---
            float intensity = Mathf.Max(Mathf.Abs(xStart), Mathf.Abs(zStart));

            // Map intensity wait time (strong wind = shorter duration)
            float waitTime = Mathf.Lerp(18f, 2f, intensity / 10f);

            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator StartNaturalWeather()
    {
        yield return new WaitForSeconds(150);

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
                // Guarantee 5 minutes of clear weather after a long rain
                if (rainLength > 480) yield return new WaitForSeconds(300);
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
