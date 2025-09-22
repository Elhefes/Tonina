using UnityEngine;

public class AudioPassController : MonoBehaviour
{
    private int minFilterValue = 666;
    private int maxFilterValue = 14000;
    private float faderTime = 2.5f;
    private float increment;
    public AudioLowPassFilter lowPassFilter;
    public bool muffleEffect;

    private void Start()
    {
        increment = (maxFilterValue - minFilterValue) / (faderTime * 100f);
    }

    public void ResetFilterValue() { lowPassFilter.cutoffFrequency = maxFilterValue; }

    private void FixedUpdate()
    {
        if (!muffleEffect && lowPassFilter.cutoffFrequency == maxFilterValue) return;

        if (muffleEffect)
        {
            if (lowPassFilter.cutoffFrequency - increment < minFilterValue)
            {
                lowPassFilter.cutoffFrequency = minFilterValue;
            }
            else lowPassFilter.cutoffFrequency -= increment;
        }
        else
        {
            if (lowPassFilter.cutoffFrequency + increment > maxFilterValue)
            {
                lowPassFilter.cutoffFrequency = maxFilterValue;
            }
            else lowPassFilter.cutoffFrequency += increment;
        }
    }
}
