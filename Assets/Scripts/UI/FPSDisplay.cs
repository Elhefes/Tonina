using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI FPSCounterText;
    private float deltaTime;

    void Update()
    {
        // Measure time between frames
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Compute FPS
        float fps = 1.0f / deltaTime;

        // Update text
        if (FPSCounterText != null)
            FPSCounterText.text = $"FPS: {fps:0.}";
    }
}
