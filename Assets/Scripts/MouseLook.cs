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
    public Player player;
    public bool cameraOnPlayer = true;
    public Camera minimapCamera;
    public GameObject minimapIndicators;

    public CameraOnPlayerButton cameraOnPlayerButton;
    public GameObject optionsMenu;
    public GameObject battleFieldMenu;
    public CameraLimiter cameraLimiter;
    public MinimapInput minimapInput;
    public float minimapInputSensitivity;

    private void Start()
    {
        // Calculate limiter lines
        cameraLimiter.BF_leftLimiterLine = (cameraLimiter.BF_LeftLimZ2 - cameraLimiter.BF_LeftLimZ1)
            / (cameraLimiter.BF_LeftLimX2 - cameraLimiter.BF_LeftLimX1);
        cameraLimiter.BF_rightLimiterLine = (cameraLimiter.BF_RightLimZ2 - cameraLimiter.BF_RightLimZ1)
            / (cameraLimiter.BF_RightLimX2 - cameraLimiter.BF_RightLimX1);

        cameraLimiter.village_leftLimiterLine = (cameraLimiter.village_LeftLimZ2 - cameraLimiter.village_LeftLimZ1)
            / (cameraLimiter.village_LeftLimX2 - cameraLimiter.village_LeftLimX1);
        cameraLimiter.village_rightLimiterLine = (cameraLimiter.village_RightLimZ2 - cameraLimiter.village_RightLimZ1)
            / (cameraLimiter.village_RightLimX2 - cameraLimiter.village_RightLimX1);
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
        if (Input.GetKeyDown("c")) CameraOnPlayerButton();
        if (Input.GetKeyDown(KeyCode.Escape) && !battleFieldMenu.activeSelf)
        {
            player.FreeTextSubject();
            if (optionsMenu.activeSelf) optionsMenu.SetActive(false);
            else optionsMenu.SetActive(true);
        }

        if (!player.insideKingHouse)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || minimapInput.GetMinimapInput().x != 0 || minimapInput.GetMinimapInput().y != 0)
            {
                CameraOnPlayerOff();
                // Invert minimap controls when player is in village
                if (player.inVillage) moveDirection = new Vector3(-Input.GetAxis("Horizontal") + minimapInput.GetMinimapInput().x * -minimapInputSensitivity, 0f, -Input.GetAxis("Vertical") + minimapInput.GetMinimapInput().y * -minimapInputSensitivity);
                else moveDirection = new Vector3(Input.GetAxis("Horizontal") + minimapInput.GetMinimapInput().x * minimapInputSensitivity, 0f, Input.GetAxis("Vertical") + minimapInput.GetMinimapInput().y * minimapInputSensitivity);

                if (player.inVillage)
                {
                    // Update the position with clamping
                    Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
                    newPosition.z = Mathf.Clamp(newPosition.z, cameraLimiter.village_ZLimit1, cameraLimiter.village_ZLimit2);
                    newPosition.x = Mathf.Clamp(newPosition.x, cameraLimiter.village_RightLimX1 - (Mathf.Abs(cameraLimiter.village_RightLimZ1) - Mathf.Abs(transform.position.z)) / cameraLimiter.village_rightLimiterLine,
                        cameraLimiter.village_LeftLimX1 - (Mathf.Abs(cameraLimiter.village_LeftLimZ1) - Mathf.Abs(transform.position.z)) / cameraLimiter.village_leftLimiterLine);
                    // General limits in x-axis
                    if (newPosition.x >= 130f) newPosition.x = 130f;
                    if (newPosition.x <= -138f) newPosition.x = -138f;
                    transform.position = newPosition;
                }
                else
                {
                    // Update the position with clamping
                    Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
                    newPosition.z = Mathf.Clamp(newPosition.z, cameraLimiter.BF_ZLimit2, cameraLimiter.BF_ZLimit1);
                    newPosition.x = Mathf.Clamp(newPosition.x, cameraLimiter.BF_LeftLimX1 + (Mathf.Abs(cameraLimiter.BF_LeftLimZ1) - Mathf.Abs(transform.position.z)) / cameraLimiter.BF_leftLimiterLine,
                        cameraLimiter.BF_RightLimX1 + (Mathf.Abs(cameraLimiter.BF_RightLimZ1) - Mathf.Abs(transform.position.z)) / cameraLimiter.BF_rightLimiterLine);
                    transform.position = newPosition;
                }
            }
        }

        // Use scroll wheel to zoom in or out
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        distanceFromObject = Mathf.Clamp(distanceFromObject - scrollWheel * sensitivity, minCameraZoom, maxCameraZoom);
        minimapCamera.orthographicSize = distanceFromObject / 2 + 20f;

        if (player == null) return;

        if (cameraOnPlayer)
        {
            if (player.inVillage)
            {
                playerToFollowAngledDirection = new Vector3(player.transform.position.x, player.transform.position.y + distanceFromObject, player.transform.position.z + (distanceFromObject / Mathf.Tan(60 * Mathf.PI / 180)) - 0.66f);
                transform.position = Vector3.Lerp(transform.position, playerToFollowAngledDirection, smoothSpeed);
                playerToFollowDirection = new Vector3(player.transform.position.x, player.transform.position.y + 100f, player.transform.position.z);
                minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, playerToFollowDirection, smoothSpeed);
            }
            else if (!player.insideKingHouse)
            {
                playerToFollowAngledDirection = new Vector3(player.transform.position.x, player.transform.position.y + distanceFromObject, player.transform.position.z - (distanceFromObject / Mathf.Tan(60 * Mathf.PI / 180)) + 0.66f);
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
        else
        {
            if (player.insideKingHouse)
            {
                ToggleCameraOnPlayer();
                return;
            }

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

    // Limit camera's movement smoothly in x-axis
    private void FixedUpdate()
    {
        // Rotate camera when entering and exiting king house
        if (player.insideKingHouse)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(60f, -90f, 0f), 1.25f);
            minimapCamera.transform.rotation = Quaternion.RotateTowards(minimapCamera.transform.rotation, Quaternion.Euler(90f, -90f, 0f), 1.25f);
            if (minimapIndicators.activeSelf) minimapIndicators.SetActive(false);
        }
        else if (player.inVillage)
        {
            if (transform.rotation.y != 0) transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(60f, 180f, 0f), 1.5f);
            minimapCamera.transform.rotation = Quaternion.RotateTowards(minimapCamera.transform.rotation, Quaternion.Euler(90f, 180f, 0f), 1.5f);
            if (!minimapIndicators.activeSelf) minimapIndicators.SetActive(true);
        }
        else
        {
            if (transform.rotation.y != 0) transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(60f, 0f, 0f), 1.5f);
            minimapCamera.transform.rotation = Quaternion.RotateTowards(minimapCamera.transform.rotation, Quaternion.Euler(90f, 0f, 0f), 1.5f);
            if (!minimapIndicators.activeSelf) minimapIndicators.SetActive(true);
        }
    }
}