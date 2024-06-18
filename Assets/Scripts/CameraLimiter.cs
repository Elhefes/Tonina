using UnityEngine;

public class CameraLimiter : MonoBehaviour
{
    [Header("General Z limits")]
    public float BF_ZLimit1;
    public float BF_ZLimit2;
    public float village_ZLimit1;
    public float village_ZLimit2;

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

    [Header("Village coordinates (X,Z) for left limiter line")]
    public float village_LeftLimX1;
    public float village_LeftLimX2;
    public float village_LeftLimZ1;
    public float village_LeftLimZ2;

    [Header("Village coordinates (X,Z) for right limiter line")]
    public float village_RightLimX1;
    public float village_RightLimX2;
    public float village_RightLimZ1;
    public float village_RightLimZ2;

    [Header("Calculated limiter line values")]
    public float BF_leftLimiterLine;
    public float BF_rightLimiterLine;
    public float village_leftLimiterLine;
    public float village_rightLimiterLine;
}
