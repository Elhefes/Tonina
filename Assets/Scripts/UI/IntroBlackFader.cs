using UnityEngine;

public class IntroBlackFader : MonoBehaviour
{
    public IntroSceneController introSceneController;

    public void StartFadeOut()
    {
        introSceneController.StartLastScene();
    }

    // This exists to prevent showing 1 frame of camera not being in last scene
    public void DisableThisObject()
    {
        gameObject.SetActive(false);
    }
}
