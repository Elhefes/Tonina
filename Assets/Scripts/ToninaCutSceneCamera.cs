using System.Collections;
using UnityEngine;

public class ToninaCutSceneCamera : MonoBehaviour
{
    public MouseLook mouseLook;
    public GameObject blackFader;
    public GameObject clickBlocker;
    public GameObject overworldUI;

    [Header("Cutscene UI Elements")]
    public GameObject generalReturnButtonObject;
    public GameObject attackModeUnlockUIObject;

    public Transform[] pyramidBuildingCameraPositions;

    private Transform previousCameraPosition;
    private Quaternion previousCameraRotation;

    private float previousRenderDistance;

    public void MoveCameraToTemporaryPosition(int extraFloorInt, Transform currentPosition, Quaternion currentRotation, float renderDistance)
    {
        StartCoroutine(StartMovingCameraToAngle(extraFloorInt, currentPosition, currentRotation, renderDistance));
    }

    public void MoveCameraToBack()
    {
        StartCoroutine(StartMovingCameraBack());
    }

    IEnumerator StartMovingCameraToAngle(int extraFloorInt, Transform camTransform, Quaternion currentRotation, float renderDistance)
    {
        mouseLook.CameraOnPlayerOff();
        mouseLook.inCutScene = true;
        overworldUI.SetActive(false);
        blackFader.SetActive(true);
        clickBlocker.SetActive(true);

        previousCameraPosition = camTransform;
        previousCameraRotation = currentRotation;

        yield return new WaitForSeconds(0.33f);

        mouseLook.notCastingRays = true;

        mouseLook.mainCameraObject.transform.position = pyramidBuildingCameraPositions[extraFloorInt].position;
        mouseLook.mainCameraObject.transform.rotation = Quaternion.Euler(pyramidBuildingCameraPositions[extraFloorInt].transform.rotation.eulerAngles);
        mouseLook.transform.position = pyramidBuildingCameraPositions[extraFloorInt].position;
        mouseLook.transform.rotation = Quaternion.Euler(pyramidBuildingCameraPositions[extraFloorInt].transform.rotation.eulerAngles);
        previousRenderDistance = renderDistance;
        Camera.main.farClipPlane = 1500; // Set render distance

        if (extraFloorInt == 0) attackModeUnlockUIObject.SetActive(true);
        else generalReturnButtonObject.SetActive(true);
    }

    IEnumerator StartMovingCameraBack()
    {
        blackFader.SetActive(true);
        yield return new WaitForSeconds(0.33f);

        mouseLook.ToggleCameraOnPlayer();
        mouseLook.inCutScene = false;
        overworldUI.SetActive(true);
        clickBlocker.SetActive(false);

        mouseLook.notCastingRays = false;

        mouseLook.mainCameraObject.transform.position = previousCameraPosition.position;
        mouseLook.mainCameraObject.transform.rotation = Quaternion.Euler(previousCameraRotation.eulerAngles);
        mouseLook.transform.position = previousCameraPosition.position;
        mouseLook.transform.rotation = Quaternion.Euler(previousCameraRotation.eulerAngles);
        Camera.main.farClipPlane = previousRenderDistance;
    }
}
