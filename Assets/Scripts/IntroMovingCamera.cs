using UnityEngine;

public class IntroMovingCamera : MonoBehaviour
{
    public Animator introCameraAnimator;
    public IntroSceneController introSceneController;

    public void SetBoolTrue(string boolName)
    {
        introCameraAnimator.SetBool(boolName, true);
    }

    public void EnablePlayerPivot() { introSceneController.EnableOnlyBattleModeObjects(); }

    public void IntroPopUpTexts() { introSceneController.introHUD_Controller.popUpTexts.SetActive(true); }
    public void FinalFade() { introSceneController.StartFinalFade(); }
    public void EndTexts() { introSceneController.StartEndTexts(); }
}
