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

    public CameraOnPlayerButton cameraOnPlayerButton;
    public GameObject optionsMenu;
    public MinimapInput minimapInput;
    public float minimapInputSensitivity;

    private Vector3 targetPosition;
    private Vector3 savedTargetPosition; // Reusable after targetPosition has been set
    private Vector3 savedTargetAngle; // Reusable after targetPosition has been set
    private bool movingToTargetPosition;

    private void Start()
    {
        if (player != null)
        {
            if (player.kingHouse != null)
            {
                if (!player.godMode) // Set correct starting position in Jadea scene only
                {
                    transform.position = player.kingHouse.transform.position + new Vector3(0f, 12f, 0f);
                    mainCameraObject.transform.position = gameObject.transform.position;
                }
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
        SaveTargetPosition(player.kingHouse.transform.position + player.kingHouse.battlefieldStartingPosition, new Vector3(60f, 0f, 0f));
        StartMovingToPosition(AngledPosition(player.kingHouse.transform.position + player.kingHouse.battlefieldStartingPosition,
                new Vector3(60f, 0f, 0f), distanceFromObject));
    }

    public void DisableBuildMode()
    {
        player.inBuildMode = false;
        cameraOnPlayer = true;
        cameraOnPlayerButton.gameObject.SetActive(true);
    }

    public void ToggleCameraOnPlayer()
    {
        cameraOnPlayer = !cameraOnPlayer;
        cameraOnPlayerButton.ChangeIconSprite(cameraOnPlayer);
    }

    public void CameraOnPlayerOff()
    {
        if (player.insideKingHouse && !inCutscene && (transform.rotation.y != 0 
            || transform.rotation.eulerAngles.y != 180)) return;
        cameraOnPlayer = false;
        cameraOnPlayerButton.ChangeIconSprite(cameraOnPlayer);

        movingToTargetPosition = false;
    }

    public void SetMovingToSpecificPosition(bool value) { movingToTargetPosition = value; }

    public void StartMovingToPosition(Vector3 pos)
    {
        targetPosition = pos;
        movingToTargetPosition = true;
    }

    public void SaveTargetPosition(Vector3 targetPosition, Vector3 targetAngle)
    {
        savedTargetPosition = targetPosition;
        savedTargetAngle = targetAngle;
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
        FollowPlayerInAngledPosition();
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

            if (!player.insideKingHouse || (player.inBuildMode && !movingToTargetPosition))
            {
                float horizontal = Input.GetAxis("Horizontal") + minimapInput.GetMinimapInput().x * minimapInputSensitivity;
                float vertical = Input.GetAxis("Vertical") + minimapInput.GetMinimapInput().y * minimapInputSensitivity;

                if (horizontal != 0f || vertical != 0f) CameraOnPlayerOff();

                // Camera-relative directions
                Vector3 forward = transform.forward;
                Vector3 right = transform.right;

                // Ignore vertical tilt
                forward.y = 0f;
                right.y = 0f;

                forward.Normalize();
                right.Normalize();

                moveDirection = right * horizontal + forward * vertical;

                if (player.inBuildMode)
                {
                    rb.AddForce(
                        moveDirection * 10200f * distanceFromObject * Time.deltaTime,
                        ForceMode.Force
                    );
                }
                else
                {
                    rb.AddForce(
                        moveDirection * 320000f * Time.deltaTime,
                        ForceMode.Force
                    );
                }
            }

            // Use scroll wheel to zoom in or out
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            distanceFromObject = Mathf.Clamp(distanceFromObject - scrollWheel * sensitivity, minCameraZoom, maxCameraZoom);
            minimapCamera.orthographicSize = distanceFromObject / 2 + 20f;
        }

        if (movingToTargetPosition && !inCutscene)
        {
            MoveToTargetPosition();
        }

        if (player == null) return;

        if (cameraOnPlayer)
        {
            FollowPlayerInAngledPosition();
        }
        else
        {
            if (player.insideKingHouse && !player.inBuildMode && !inCutscene)
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

    private Vector3 AngledPosition(Vector3 targetPosition, Vector3 targetAngle, float distanceFromObject)
    {
        Quaternion rotation = Quaternion.Euler(targetAngle);

        // Direction the camera is looking.
        Vector3 forward = rotation * Vector3.forward;

        // Camera sits behind the target along its viewing direction.
        return targetPosition - forward * distanceFromObject;
    }

    void FollowPlayerInAngledPosition()
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

        // Small positional offset (+0.66f / -0.66f)
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

    private void MoveToTargetPosition()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * 0.5f);
        minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, targetPosition, smoothSpeed * 0.5f);
        UpdateTargetPosition(); // Updates target position with distanceFromObject
        if (Vector3.Distance(transform.position, targetPosition) <= 0.5f) movingToTargetPosition = false;
    }

    private void UpdateTargetPosition()
    {
        targetPosition = AngledPosition(savedTargetPosition, savedTargetAngle, distanceFromObject);
    }

    private void FixedUpdate()
    {
        if (player.inVillage && inCutscene) return; // In village = not in battlefield = not during 1st attack

        RotateSmoothly(presetCameraAngle);

        if (player.insideKingHouse && minimapIndicators.activeSelf) minimapIndicators.SetActive(false);
        else if (!minimapIndicators.activeSelf && !minimapInput.buttonPressed) minimapIndicators.SetActive(true);
    }

    public void SetInCutscene(bool value) { inCutscene = value; }

    private void RotateSmoothly(Vector3 presetPosition)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(presetPosition), 1.25f);
        mainCameraObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(presetPosition), 1.25f);
        minimapCamera.transform.rotation = Quaternion.RotateTowards(minimapCamera.transform.rotation,
            Quaternion.Euler(new Vector3(90f, presetPosition.y, 0f)), 1.25f);
    }
}