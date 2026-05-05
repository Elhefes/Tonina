using UnityEngine;

public class AudioPassController : MonoBehaviour
{
    private int minFilterValue;
    private int maxFilterValue;
    private float faderTime;
    private float increment;
    public AudioLowPassFilter lowPassFilter;
    public bool muffleEffect;

    private void Start()
    {
        minFilterValue = 666;
        maxFilterValue = 14000;
        faderTime = 2.5f;
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
