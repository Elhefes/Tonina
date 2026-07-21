using System.Collections;
using UnityEngine;

public class CutsceneCamera : MonoBehaviour
{
    // There has to be a separate camera for animations because animator causes movement issues for main camera movement
    public Camera animatedCamera;
    public Camera mainCamera;
    public Animator animatedCameraAnimator;

    public MouseLook mouseLook;
    public GameObject blackFader;
    public GameObject clickBlocker;
    public GameObject overworldUI;

    [Header("Animation: First Battle")]
    public GameObject firstBattleObject;

    [Header("Static Cutscene UI Elements")]
    public GameObject generalReturnButtonObject;
    public GameObject attackModeUnlockUIObject;

    [Header("Static Cam Positions")]
    public Transform attributeUnlockCameraPosition;
    public Transform[] pyramidFloorCameraPositions;
    public Transform[] pyramidBuildingCameraPositions;

    private Transform previousCameraPosition;
    private Quaternion previousCameraRotation;

    private float previousRenderDistance;

    public void StartAnimation(string triggerName)
    {
        if (triggerName == "FirstBattle") firstBattleObject.SetActive(true);

        animatedCameraAnimator.SetTrigger(triggerName);
        StartCoroutine(SwitchToAnimatedCamera());

        mouseLook.CameraOnPlayerOff();
        mouseLook.inCutscene = true;
        overworldUI.SetActive(false);
        blackFader.SetActive(true);
        clickBlocker.SetActive(true);
    }

    public void ReturnFromAnimation()
    {
        mainCamera.gameObject.tag = "MainCamera";
        mainCamera.enabled = true;
        animatedCamera.gameObject.tag = "Untagged";
        animatedCamera.enabled = false;

        overworldUI.SetActive(true);
        if (!mouseLook.cameraOnPlayer) mouseLook.ToggleCameraOnPlayer();
        mouseLook.inCutscene = false;
        clickBlocker.SetActive(false);
    }

    IEnumerator SwitchToAnimatedCamera()
    {
        yield return new WaitForSeconds(0.33f);
        animatedCamera.gameObject.tag = "MainCamera";
        animatedCamera.enabled = true;
        mainCamera.gameObject.tag = "Untagged";
        mainCamera.enabled = false;
    }

    private void SetToCutsceneMode(bool value)
    {
        mouseLook.inCutscene = value;
        if (value) mouseLook.CameraOnPlayerOff();
        else mouseLook.ToggleCameraOnPlayer();
        overworldUI.SetActive(!value);
        clickBlocker.SetActive(value);
    }

    public void MoveCameraToAttributeUnlockPosition(Transform currentPosition, Quaternion currentRotation)
    {
        StartCoroutine(StartMovingToAttributeUnlockPosition(currentPosition, currentRotation));
    }

    public void MoveCameraToTemporaryPyramidPosition(bool singleBuilding, int extraFloorInt, Transform currentPosition, Quaternion currentRotation, float renderDistance)
    {
        StartCoroutine(StartMovingCameraToPyramidAngle(singleBuilding, extraFloorInt, currentPosition, currentRotation, renderDistance));
    }

    public void MoveCameraBack()
    {
        StartCoroutine(StartMovingCameraBack());
    }

    IEnumerator StartMovingToAttributeUnlockPosition(Transform camTransform, Quaternion currentRotation)
    {
        SetToCutsceneMode(true);
        blackFader.SetActive(true);

        previousCameraPosition = camTransform;
        previousCameraRotation = currentRotation;

        yield return new WaitForSeconds(0.33f);

        mouseLook.notCastingRays = true;

        mouseLook.mainCameraObject.transform.position = attributeUnlockCameraPosition.position;
        mouseLook.mainCameraObject.transform.rotation = Quaternion.Euler(attributeUnlockCameraPosition.rotation.eulerAngles);
        mouseLook.transform.position = attributeUnlockCameraPosition.position;
        mouseLook.transform.rotation = Quaternion.Euler(attributeUnlockCameraPosition.rotation.eulerAngles);
    }

    IEnumerator StartMovingCameraToPyramidAngle(bool singleBuilding, int extraFloorInt, Transform camTransform, Quaternion currentRotation, float renderDistance)
    {
        SetToCutsceneMode(true);
        blackFader.SetActive(true);

        previousCameraPosition = camTransform;
        previousCameraRotation = currentRotation;

        yield return new WaitForSeconds(0.33f);

        mouseLook.notCastingRays = true;

        if (singleBuilding)
        {
            mouseLook.mainCameraObject.transform.position = pyramidBuildingCameraPositions[extraFloorInt].position;
            mouseLook.mainCameraObject.transform.rotation = Quaternion.Euler(pyramidBuildingCameraPositions[extraFloorInt].transform.rotation.eulerAngles);
            mouseLook.transform.position = pyramidBuildingCameraPositions[extraFloorInt].position;
            mouseLook.transform.rotation = Quaternion.Euler(pyramidBuildingCameraPositions[extraFloorInt].transform.rotation.eulerAngles);
        }
        else
        {
            mouseLook.mainCameraObject.transform.position = pyramidFloorCameraPositions[extraFloorInt].position;
            mouseLook.mainCameraObject.transform.rotation = Quaternion.Euler(pyramidFloorCameraPositions[extraFloorInt].transform.rotation.eulerAngles);
            mouseLook.transform.position = pyramidFloorCameraPositions[extraFloorInt].position;
            mouseLook.transform.rotation = Quaternion.Euler(pyramidFloorCameraPositions[extraFloorInt].transform.rotation.eulerAngles);
        }
        
        previousRenderDistance = renderDistance;
        Camera.main.farClipPlane = 140; // Set render distance

        if (extraFloorInt == 0 && !singleBuilding) attackModeUnlockUIObject.SetActive(true);
        else generalReturnButtonObject.SetActive(true);
    }

    IEnumerator StartMovingCameraBack()
    {
        blackFader.SetActive(true);
        yield return new WaitForSeconds(0.33f);

        SetToCutsceneMode(false);

        mouseLook.notCastingRays = false;

        mouseLook.mainCameraObject.transform.position = previousCameraPosition.position;
        mouseLook.mainCameraObject.transform.rotation = Quaternion.Euler(previousCameraRotation.eulerAngles);
        mouseLook.transform.position = previousCameraPosition.position;
        mouseLook.transform.rotation = Quaternion.Euler(previousCameraRotation.eulerAngles);
        Camera.main.farClipPlane = previousRenderDistance;
    }
}
