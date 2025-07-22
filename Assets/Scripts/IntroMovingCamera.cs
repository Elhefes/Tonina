using UnityEngine;

public class IntroMovingCamera : MonoBehaviour
{
    public Animator introCameraAnimator;
    public IntroSceneController introSceneController;

    public void SetBoolTrue(string boolName)
    {
        introCameraAnimator.SetBool(boolName, true);
    }

    public void EnablePlayerPivot()
    {
        introSceneController.EnableOnlyBattleModeObjects();
    }

    public void FinalFade()
    {
        introSceneController.StartFinalFade();
    }
}
