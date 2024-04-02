using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float fps;
    public TMPro.TextMeshProUGUI FPSCounterText;

    void Start()
    {
        InvokeRepeating("GetFPS", 0.5f, 0.5f);
    }

    void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        FPSCounterText.text = "FPS = " + fps.ToString();
    }
}
