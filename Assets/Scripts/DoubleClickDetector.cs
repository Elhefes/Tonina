using UnityEngine;

public class DoubleClickDetector : MonoBehaviour
{
    private float clicked = 0;
    private float clickTime = 0;
    private float clickDelay = 0.67f;

    public void AddClick() { clicked++; }

    public bool DoubleClickDetected()
    {
        if (clicked == 1)
            clickTime = Time.time;

        if (clicked > 1 && Time.time - clickTime < clickDelay)
        {
            // Double click detected
            clicked = 0;
            clickTime = 0;
            return true;
        }
        else if (clicked > 2 || Time.time - clickTime > 1)
            clicked = 0;
        return false;
    }
}
