using System.Collections;
using UnityEngine;

public class ToninaCutSceneCamera : MonoBehaviour
{
    public MouseLook mouseLook;
    public GameObject blackFader;
    public GameObject overworldUI;

    public Transform[] pyramidBuildingCameraPositions;
    //public Quaternion[] pyramidBuildingCameraRotations;

    private Transform previousCameraPosition;
    private Quaternion previousCameraRotation;

    public void MoveCameraToTemporaryPosition(int extraFloorInt, Transform currentPosition, Quaternion currentRotation)
    {
        StartCoroutine(StartMovingCameraToAngle(extraFloorInt, currentPosition, currentRotation));
    }

    public void MoveCameraToBack()
    {
        StartCoroutine(StartMovingCameraBack());
    }

    IEnumerator StartMovingCameraToAngle(int extraFloorInt, Transform camTransform, Quaternion currentRotation)
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
        Camera.main.farClipPlane = 1500; // Set render distance
    }

    IEnumerator StartMovingCameraBack()
    {
        blackFader.SetActive(true);
        yield return new WaitForSeconds(0.33f);

        mouseLook.inCutScene = false;

        Camera.main.transform.position = previousCameraPosition.position;
        Camera.main.transform.rotation = previousCameraRotation;
    }
}
