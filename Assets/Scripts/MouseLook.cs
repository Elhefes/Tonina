using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject mainCameraObject;
    public float moveSpeed;
    public float sensitivity;
    public LayerMask layerMask;
    private Vector3 moveDirection;

    public Vector3 presetCameraAngle; // Preset in scene

    private Vector3 playerToFollowAngledDirection;
    private Vector3 playerToFollowDirection;
    private Vector3 freeMinimapCameraDirection;
    public float distanceFromObject;
    private float currentDistance;
    public float smoothSpeed;
    public float minCameraZoom;
    public float maxCameraZoom;
    public Player player;
    public bool inCutscene;
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

    private void Start()
    {
        if (player != null)
        {
            if (player.kingHouse != null && !player.godMode) // Set correct starting position in Jadea scene only
            {
                transform.position = player.kingHouse.transform.position + new Vector3(0f, 12f, 0f);
                mainCameraObject.transform.position = gameObject.transform.position;
            }
        }
    }

    public void SetCameraAngle(Vector3 angle)
    {
        presetCameraAngle = angle;
    }

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

    public void ToggleCameraOnPlayer()
    {
        cameraOnPlayer = !cameraOnPlayer;
        cameraOnPlayerButton.ChangeIconSprite(cameraOnPlayer);
    }

    public void CameraOnPlayerOff()
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
        CalculateAngledCameraPosition();
        mainCameraObject.transform.position = playerToFollowAngledDirection;
        gameObject.transform.position = playerToFollowAngledDirection;
        minimapCamera.transform.position = playerToFollowDirection;
    }

    void Update()
    {
        mainCameraObject.transform.position = Vector3.Lerp(mainCameraObject.transform.position, transform.position, smoothSpeed);

        if (!inCutscene)
        {
            if (!player.inBuildMode && !notCastingRays)
            {
                if (Input.GetKeyDown("c")) ToggleCameraOnPlayer();
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
            CalculateAngledCameraPosition();
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

    void CalculateAngledCameraPosition()
    {
        float xRotation = 60f;

        // Horizontal ground distance caused by 60 degree tilt
        float horizontalDistance =
            distanceFromObject / Mathf.Tan(xRotation * Mathf.Deg2Rad);

        // Current camera Y rotation
        float yRotation = transform.rotation.eulerAngles.y;

        // Direction the camera moves backwards on the ground plane
        Quaternion rotation = Quaternion.Euler(0f, yRotation, 0f);

        Vector3 backwardOffset = rotation * Vector3.back * horizontalDistance;

        // Small positional offset you previously had (+0.66f / -0.66f)
        Vector3 extraOffset = rotation * Vector3.back * -0.66f;

        // Final camera target position
        playerToFollowAngledDirection =
            player.transform.position
            + Vector3.up * distanceFromObject
            + backwardOffset
            + extraOffset;

        // Smooth movement
        Vector3 newPos = Vector3.Lerp(
            transform.position,
            playerToFollowAngledDirection,
            smoothSpeed
        );

        // King house X clamp
        if (player.insideKingHouse)
        {
            newPos.x = Mathf.Min(newPos.x, 7.5f);
        }

        transform.position = newPos;

        // Keep camera looking downward with rotatable Y angle
        transform.rotation = Quaternion.Euler(
            xRotation,
            yRotation,
            0f
        );

        // Minimap follow
        playerToFollowDirection = new Vector3(
            player.transform.position.x,
            player.transform.position.y + 100f,
            player.transform.position.z
        );

        minimapCamera.transform.position = Vector3.Lerp(
            minimapCamera.transform.position,
            playerToFollowDirection,
            smoothSpeed
        );
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
        if (player.inVillage && inCutscene) return;

        // Rotate camera to camera angle which is preset in scene
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(presetCameraAngle), 1.25f);
        mainCameraObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(presetCameraAngle), 1.25f);
        minimapCamera.transform.rotation = Quaternion.RotateTowards(minimapCamera.transform.rotation, 
            Quaternion.Euler(new Vector3(90f, presetCameraAngle.y, 0f)), 1.25f);

        if (player.insideKingHouse && minimapIndicators.activeSelf) minimapIndicators.SetActive(false);
        else if (!minimapIndicators.activeSelf && !minimapInput.buttonPressed) minimapIndicators.SetActive(true);
    }

    public void SetInCutscene(bool value) { inCutscene = value; }
}