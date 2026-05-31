using System.Collections;
using UnityEngine;

public class ChichenItzaCutsceneCamera : MonoBehaviour
{
    public MouseLook mouseLook;
    public Camera cutsceneCamera;
    public Camera gameplayCamera;
    public FriendlyCluster friendlyCluster;
    private float originalFriendliesSpeed;
    public GameObject battleUI;
    public GameObject clickBlocker;

    private void Start()
    {
        StartCoroutine(SwitchMainCameraToCutsceneCamera());
        if (friendlyCluster != null) originalFriendliesSpeed = friendlyCluster.GetFriendlySpeed();
        friendlyCluster.SetFriendlySpeeds(0f);
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
        friendlyCluster.SetFriendlySpeeds(originalFriendliesSpeed);
    }
}
