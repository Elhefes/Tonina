using UnityEngine;

public class CameraLimiter : MonoBehaviour
{
    [Header("General Z limits")]
    public float BF_ZLimit1;
    public float BF_ZLimit2;
    public float villageZLimit1;
    public float villageZLimit2;

    [Header("Battlefield coordinates (X,Z) for left limiter line")]
    public float BF_LeftLimX1;
    public float BF_LeftLimX2;
    public float BF_LeftLimZ1;
    public float BF_LeftLimZ2;

    [Header("Battlefield coordinates (X,Z) for right limiter line")]
    public float BF_RightLimX1;
    public float BF_RightLimX2;
    public float BF_RightLimZ1;
    public float BF_RightLimZ2;

    [Header("Calculated limiter line values")]
    public float leftLimiterLine;
    public float rightLimiterLine;
}
