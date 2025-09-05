using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject mainCameraObject;
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
    public Player player;
    public bool inCutScene;
    public bool cameraOnPlayer;
    public bool notCastingRays; // Don't cast rays in Attack Mode Minimap view
    public Camera minimapCamera;
    public GameObject minimapIndicators;

    public GameObject placeablesParent;
    private bool transitioningToBuildModeAngle;

    public CameraOnPlayerButton cameraOnPlayerButton;
    public GameObject optionsMenu;
    public GameObject battleFieldMenu;
    public MinimapInput minimapInput;
    public float minimapInputSensitivity;

    private Vector3 specificPosition;
    private bool movingToSpecificPosition;

    public void EnableBuildMode()
    {
        player.inBuildMode = true;
        cameraOnPlayer = false;
        cameraOnPlayerButton.gameObject.SetActive(false);
        transitioningToBuildModeAngle = true;
    }

    public void DisableBuildMode()
    {
        player.inBuildMode = false;
        cameraOnPlayer = true;
        cameraOnPlayerButton.gameObject.SetActive(true);
        transitioningToBuildModeAngle = false;
    }

    public void CameraOnPlayerButton()
    {
        // This check can be buggy, but it likely won't matter with button calls
        if (transform.rotation.y != 0 && transform.rotation.eulerAngles.y != 180) return;
        ToggleCameraOnPlayer();
    }

    public void ToggleCameraOnPlayer()
    {
        cameraOnPlayer = !cameraOnPlayer;
        cameraOnPlayerButton.ChangeIconSprite(cameraOnPlayer);
    }

    void CameraOnPlayerOff()
    {
        if (player.insideKingHouse && (transform.rotation.y != 0 || transform.rotation.eulerAngles.y != 180)) return;
        cameraOnPlayer = false;
        cameraOnPlayerButton.ChangeIconSprite(cameraOnPlayer);

        movingToSpecificPosition = false;
    }

    public void SetMovingToSpecificPosition(bool value) { movingToSpecificPosition = value; }

    public void StartMovingToPosition(Vector3 pos)
    {
        specificPosition = pos;
        movingToSpecificPosition = true;
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

    public void TeleportCameras()
    {
        // Teleports cameras to where the cameras should be after teleport
        CalculatePlayerToFollowAngledDirection();
        mainCameraObject.transform.position = playerToFollowAngledDirection;
        gameObject.transform.position = playerToFollowAngledDirection;
        minimapCamera.transform.position = playerToFollowDirection;
    }

    void Update()
    {
        mainCameraObject.transform.position = Vector3.Lerp(mainCameraObject.transform.position, transform.position, smoothSpeed);

        if (!inCutScene)
        {
            if (!player.inBuildMode)
            {
                if (Input.GetKeyDown("c")) CameraOnPlayerButton();
            }

            if (!player.insideKingHouse || (player.inBuildMode && !transitioningToBuildModeAngle))
            {
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || minimapInput.GetMinimapInput().x != 0 || minimapInput.GetMinimapInput().y != 0)
                {
                    CameraOnPlayerOff();
                    // Invert minimap controls when player is in village
                    if (player.inVillage) moveDirection = new Vector3(-Input.GetAxis("Horizontal") + minimapInput.GetMinimapInput().x * -minimapInputSensitivity, 0f, -Input.GetAxis("Vertical") + minimapInput.GetMinimapInput().y * -minimapInputSensitivity);
                    else moveDirection = new Vector3(Input.GetAxis("Horizontal") + minimapInput.GetMinimapInput().x * minimapInputSensitivity, 0f, Input.GetAxis("Vertical") + minimapInput.GetMinimapInput().y * minimapInputSensitivity);

                    if (player.inBuildMode)
                    {
                        rb.AddForce(moveDirection * 10200f * distanceFromObject * Time.deltaTime, ForceMode.Force);
                    }
                    else
                    {
                        rb.AddForce(moveDirection * 320000f * Time.deltaTime, ForceMode.Force);
                    }
                }
            }

            // Use scroll wheel to zoom in or out
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            distanceFromObject = Mathf.Clamp(distanceFromObject - scrollWheel * sensitivity, minCameraZoom, maxCameraZoom);
            minimapCamera.orthographicSize = distanceFromObject / 2 + 20f;
        }

        if (movingToSpecificPosition)
        {
            MoveToSpecificPosition();
        }

        if (player == null) return;

        if (cameraOnPlayer || transitioningToBuildModeAngle)
        {
            CalculatePlayerToFollowAngledDirection();
        }
        else
        {
            if (player.insideKingHouse && !player.inBuildMode)
            {
                ToggleCameraOnPlayer();
                return;
            }

            freeMinimapCameraDirection = new Vector3(transform.position.x, transform.position.y + 100f, transform.position.z);
            minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, freeMinimapCameraDirection, smoothSpeed);

            if (!notCastingRays)
            {
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
                        rb.AddForce((targetPosition - rb.position) * 250000f * Time.deltaTime, ForceMode.Force);
                    }
                    else if (currentDistance > distanceFromObject)
                    {
                        // If the camera is too far from the object, move it forwards smoothly
                        Vector3 targetPosition = hit.point + (transform.position - hit.point).normalized * distanceFromObject;
                        rb.AddForce((targetPosition - rb.position) * 250000f * Time.deltaTime, ForceMode.Force);
                    }
                }

                // Placeables parent follows the center of the camera
                if (player.inBuildMode)
                {
                    if (placeablesParent != null)
                    {
                        placeablesParent.transform.position = hit.point;
                    }
                }
            }
        }
    }

    void CalculatePlayerToFollowAngledDirection()
    {
        if (player.inBuildMode)
        {
            // This assumes that King House's x = 0 always, it makes sense design-wise

            Vector3 kingHouseEntrancePosition = new Vector3(0f, player.kingHouse.transform.position.y, player.kingHouse.transform.position.z - 11.8f);

            playerToFollowAngledDirection = new Vector3(0f, kingHouseEntrancePosition.y + distanceFromObject, kingHouseEntrancePosition.z - (distanceFromObject / Mathf.Tan(60 * Mathf.PI / 180)));
            transform.position = Vector3.Lerp(transform.position, playerToFollowAngledDirection, smoothSpeed * distanceFromObject / 10f);
            playerToFollowDirection = new Vector3(0f, kingHouseEntrancePosition.y + 100f, kingHouseEntrancePosition.z);
            minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, playerToFollowDirection, smoothSpeed);

            // The camera doesn't have time to get to x = 0 before rotation y = 0, so the x is a little bit off 0
            if (transform.rotation.eulerAngles.y == 0f && (Mathf.Abs(transform.position.x) <= 0.08f)) transitioningToBuildModeAngle = false;
        }
        else if (!player.inVillage && !player.insideKingHouse) // In battle mode
        {
            playerToFollowAngledDirection = new Vector3(player.transform.position.x, player.transform.position.y + distanceFromObject, player.transform.position.z - (distanceFromObject / Mathf.Tan(60 * Mathf.PI / 180)) + 0.66f);
            transform.position = Vector3.Lerp(transform.position, playerToFollowAngledDirection, smoothSpeed);
            playerToFollowDirection = new Vector3(player.transform.position.x, player.transform.position.y + 100f, player.transform.position.z);
            minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, playerToFollowDirection, smoothSpeed);
        }
        else if (player.inVillage)
        {
            playerToFollowAngledDirection = new Vector3(player.transform.position.x, player.transform.position.y + distanceFromObject, player.transform.position.z + (distanceFromObject / Mathf.Tan(60 * Mathf.PI / 180)) - 0.66f);
            transform.position = Vector3.Lerp(transform.position, playerToFollowAngledDirection, smoothSpeed);
            playerToFollowDirection = new Vector3(player.transform.position.x, player.transform.position.y + 100f, player.transform.position.z);
            minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, playerToFollowDirection, smoothSpeed);
        }
        else
        {
            playerToFollowAngledDirection = new Vector3(player.transform.position.x + (distanceFromObject / Mathf.Tan(60 * Mathf.PI / 180)) - 0.66f, player.transform.position.y + distanceFromObject, player.transform.position.z);
            transform.position = Vector3.Lerp(transform.position, playerToFollowAngledDirection, smoothSpeed);
            playerToFollowDirection = new Vector3(player.transform.position.x, player.transform.position.y + 100f, player.transform.position.z);
            minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, playerToFollowDirection, smoothSpeed);
        }
    }

    private void MoveToSpecificPosition()
    {
        if (specificPosition != null)
        {
            transform.position = Vector3.Lerp(transform.position, specificPosition, smoothSpeed * 0.5f);
            minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, specificPosition, smoothSpeed * 0.5f);
        }
    }

    // Limit camera's movement smoothly in x-axis
    private void FixedUpdate()
    {
        if (player.inBuildMode)
        {
            RotateCameraToBattlefieldAngle();
        }

        // Rotate camera when entering and exiting king house
        else if (player.insideKingHouse)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(60f, -90f, 0f), 1.25f);
            mainCameraObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(60f, -90f, 0f), 1.25f);
            minimapCamera.transform.rotation = Quaternion.RotateTowards(minimapCamera.transform.rotation, Quaternion.Euler(90f, -90f, 0f), 1.25f);
            if (minimapIndicators.activeSelf) minimapIndicators.SetActive(false);
        }
        else if (player.inVillage)
        {
            if (transform.rotation.y != 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(60f, 180f, 0f), 1.5f);
                mainCameraObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(60f, 180f, 0f), 1.5f);
            }
            minimapCamera.transform.rotation = Quaternion.RotateTowards(minimapCamera.transform.rotation, Quaternion.Euler(90f, 180f, 0f), 1.5f);
            if (!minimapIndicators.activeSelf && !minimapInput.buttonPressed) minimapIndicators.SetActive(true);
        }

        else RotateCameraToBattlefieldAngle();
    }

    void RotateCameraToBattlefieldAngle()
    {
        if (transform.rotation.y != 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(60f, 0f, 0f), 1.5f);
            mainCameraObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(60f, 0f, 0f), 1.5f);
        }
        minimapCamera.transform.rotation = Quaternion.RotateTowards(minimapCamera.transform.rotation, Quaternion.Euler(90f, 0f, 0f), 1.5f);
        if (!minimapIndicators.activeSelf && !minimapInput.buttonPressed) minimapIndicators.SetActive(true);
    }

    public void SetInCutScene(bool value) { inCutScene = value; }
}