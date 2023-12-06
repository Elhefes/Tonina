using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float moveSpeed;
    public float sensitivity;
    public LayerMask layerMask;
    private Vector3 moveDirection;
    private Vector3 playerToFollowAngledDirection;
    private Vector3 playerToFollowDirection;
    private Vector3 freeMinimapCameraDirection;
    public float distanceFromObject;
    private float currentDistance;
    public float smoothSpeed;
    public float minCameraZoom;
    public float maxCameraZoom;
    public GameObject playerToFollow;
    public bool cameraOnPlayer = true;
    public Camera minimapCamera;

    public CameraOnPlayerButton cameraOnPlayerButton;
    public MinimapInput minimapInput;
    public float minimapInputSensitivity;

    public void ToggleCameraOnPlayer()
    {
        cameraOnPlayer = !cameraOnPlayer;
        cameraOnPlayerButton.ChangeIconSprite(cameraOnPlayer);
    }

    void CameraOnPlayerOff()
    {
        cameraOnPlayer = false;
        cameraOnPlayerButton.ChangeIconSprite(cameraOnPlayer);
    }

    public void ZoomCameraInOrOut(bool zoom_in)
    {
        if (zoom_in)
        {
            distanceFromObject = Mathf.Clamp(distanceFromObject - 5f, minCameraZoom, maxCameraZoom);
        }
        else
        {
            distanceFromObject = Mathf.Clamp(distanceFromObject + 5f, minCameraZoom, maxCameraZoom);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c")) ToggleCameraOnPlayer();

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || minimapInput.GetMinimapInput().x != 0 || minimapInput.GetMinimapInput().y != 0)
        {
            CameraOnPlayerOff();
            moveDirection = new Vector3(Input.GetAxis("Horizontal") + minimapInput.GetMinimapInput().x * minimapInputSensitivity, 0f, Input.GetAxis("Vertical") + minimapInput.GetMinimapInput().y * minimapInputSensitivity);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        // Use scroll wheel to zoom in or out
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        distanceFromObject = Mathf.Clamp(distanceFromObject - scrollWheel * sensitivity, minCameraZoom, maxCameraZoom);
        minimapCamera.orthographicSize = distanceFromObject / 2 + 20f;

        if (cameraOnPlayer)
        {
            playerToFollowAngledDirection = new Vector3(playerToFollow.transform.position.x, playerToFollow.transform.position.y + distanceFromObject, playerToFollow.transform.position.z - (distanceFromObject / Mathf.Tan(60 * Mathf.PI / 180)) + 0.66f);
            transform.position = Vector3.Lerp(transform.position, playerToFollowAngledDirection, smoothSpeed);
            playerToFollowDirection = new Vector3(playerToFollow.transform.position.x, playerToFollow.transform.position.y + 100f, playerToFollow.transform.position.z);
            minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, playerToFollowDirection, smoothSpeed);

        }
        else
        {
            freeMinimapCameraDirection = new Vector3(transform.position.x, transform.position.y + 100f, transform.position.z);
            minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, freeMinimapCameraDirection, smoothSpeed);
            // Cast a ray downwards from the camera's position
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                currentDistance = Vector3.Distance(transform.position, hit.point);
                if (currentDistance < distanceFromObject)
                {
                    // If the camera is too close to the object, move it backwards smoothly
                    Vector3 targetPosition = hit.point + (transform.position - hit.point).normalized * distanceFromObject;
                    transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
                }
                else if (currentDistance > distanceFromObject)
                {
                    // If the camera is too far from the object, move it forwards smoothly
                    Vector3 targetPosition = hit.point + (transform.position - hit.point).normalized * distanceFromObject;
                    transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
                }
            }
        }
    }
}