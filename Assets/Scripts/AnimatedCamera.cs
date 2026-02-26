using UnityEngine;

public class AnimatedCamera : MonoBehaviour
{
    public CutsceneCamera cutsceneCamera;

    public void EndCameraAnimation()
    {
        cutsceneCamera.ReturnFromAnimation();
    }
}
