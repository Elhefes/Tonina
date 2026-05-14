using UnityEngine;

public class GameplayCameraAngles : MonoBehaviour
{
    public Transform[] presetCameraAngles;
    public MouseLook mouseLook;

    public void SetCameraAngle(int camAngleIndex)
    {
        if (mouseLook != null) mouseLook.SetCameraAngle(presetCameraAngles[camAngleIndex].rotation.eulerAngles);
    }
}
