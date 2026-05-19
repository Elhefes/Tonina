using System.Collections;
using UnityEngine;

public class ChichenItzaCutsceneCamera : MonoBehaviour
{
    public MouseLook mouseLook;
    public Camera cutsceneCamera;
    public Camera gameplayCamera;
    public GameObject battleUI;
    public GameObject clickBlocker;

    private void Awake()
    {
        StartCoroutine(SwitchMainCameraToCutsceneCamera());
    }

    IEnumerator SwitchMainCameraToCutsceneCamera()
    {
        yield return new WaitForEndOfFrame();
        cutsceneCamera.gameObject.tag = "MainCamera";
        cutsceneCamera.enabled = true;
        gameplayCamera.gameObject.tag = "Untagged";
        gameplayCamera.enabled = false;
    }

    private void SwitchCutsceneCameraToMainCamera()
    {
        gameplayCamera.gameObject.tag = "MainCamera";
        gameplayCamera.enabled = true;
        cutsceneCamera.gameObject.tag = "Untagged";
        cutsceneCamera.enabled = false;
    }

    public void EndStartScene()
    {
        mouseLook.inCutscene = false;
        battleUI.SetActive(true);
        cutsceneCamera.enabled = false;
        gameplayCamera.enabled = true;
        SwitchCutsceneCameraToMainCamera();
        clickBlocker.SetActive(false);
        gameObject.SetActive(false);
    }
}
