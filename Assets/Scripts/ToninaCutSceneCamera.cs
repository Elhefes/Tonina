using System.Collections;
using UnityEngine;

public class ToninaCutSceneCamera : MonoBehaviour
{
    public MouseLook mouseLook;
    public GameObject blackFader;
    public GameObject overworldUI;

    [Header("Cutscene UI Elements")]
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
        yield return new WaitForSeconds(0.33f);

        previousCameraPosition = camTransform;
        previousCameraRotation = currentRotation;

        mouseLook.notCastingRays = true;

        mouseLook.mainCameraObject.transform.position = pyramidBuildingCameraPositions[extraFloorInt].position;
        mouseLook.mainCameraObject.transform.rotation = Quaternion.Euler(pyramidBuildingCameraPositions[extraFloorInt].transform.rotation.eulerAngles);
        mouseLook.transform.position = pyramidBuildingCameraPositions[extraFloorInt].position;
        mouseLook.transform.rotation = Quaternion.Euler(pyramidBuildingCameraPositions[extraFloorInt].transform.rotation.eulerAngles);
        previousRenderDistance = renderDistance;
        Camera.main.farClipPlane = 1500; // Set render distance

        if (extraFloorInt == 0) attackModeUnlockUIObject.SetActive(true);
    }

    IEnumerator StartMovingCameraBack()
    {
        blackFader.SetActive(true);
        yield return new WaitForSeconds(0.33f);

        mouseLook.ToggleCameraOnPlayer();
        mouseLook.inCutScene = false;
        overworldUI.SetActive(true);

        mouseLook.notCastingRays = false;

        Camera.main.transform.position = previousCameraPosition.position;
        Camera.main.transform.rotation = previousCameraRotation;
        Camera.main.farClipPlane = previousRenderDistance;
    }
}
