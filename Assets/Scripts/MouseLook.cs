using UnityEngine;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{
    public float moveSpeed;
    public float sensitivity;
    public LayerMask layerMask;
    private Vector3 moveDirection;
    private Vector3 playerToFollowAngledDirection;
    private Vector3 playerToFollowDirection;
    private Vector3 freeMinimapCameraDirection;
    private Vector3 xDirection;
    public float distanceFromObject;
    private float currentDistance;
    public float smoothSpeed;
    public float minCameraZoom;
    public float maxCameraZoom;
    private bool onXLim;
    public Player player;
    public bool cameraOnPlayer = true;
    public Camera minimapCamera;

    public CameraOnPlayerButton cameraOnPlayerButton;
    public GameObject optionsMenu;
    public CameraLimiter cameraLimiter;
    public MinimapInput minimapInput;
    public float minimapInputSensitivity;
    private float acceleration;

    float minZ = -142f;
    float maxZ = -80f;

    public void ToggleCameraOnPlayer()
    {
        if (transform.rotation.y != 0) return;
        cameraOnPlayer = !cameraOnPlayer;
        cameraOnPlayerButton.ChangeIconSprite(cameraOnPlayer);
    }

    void CameraOnPlayerOff()
    {
        if (player.insideKingHouse || transform.rotation.y != 0) return;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ClickBlocker"))
        {
            cameraLimiter = other.GetComponent<CameraLimiter>();
            if (cameraLimiter != null)
            {
                onXLim = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c")) ToggleCameraOnPlayer();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenu.activeSelf) optionsMenu.SetActive(false);
            else optionsMenu.SetActive(true);
        }

        if (!player.insideKingHouse)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || minimapInput.GetMinimapInput().x != 0 || minimapInput.GetMinimapInput().y != 0)
            {
                CameraOnPlayerOff();
                moveDirection = new Vector3(Input.GetAxis("Horizontal") + minimapInput.GetMinimapInput().x * minimapInputSensitivity, 0f, Input.GetAxis("Vertical") + minimapInput.GetMinimapInput().y * minimapInputSensitivity);

                // Update the position with clamping
                Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
                newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ); // Adjust minZ and maxZ as needed
                if (transform.position.z < minZ - 0.5f || transform.position.z > maxZ + 0.5f) transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
                else transform.position = newPosition;
            }
        }

        // Use scroll wheel to zoom in or out
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        distanceFromObject = Mathf.Clamp(distanceFromObject - scrollWheel * sensitivity, minCameraZoom, maxCameraZoom);
        minimapCamera.orthographicSize = distanceFromObject / 2 + 20f;

        if (player == null) return;

        if (cameraOnPlayer)
        {
            if (!player.insideKingHouse)
            {
                playerToFollowAngledDirection = new Vector3(player.transform.position.x, player.transform.position.y + distanceFromObject, player.transform.position.z - (distanceFromObject / Mathf.Tan(60 * Mathf.PI / 180)) + 0.66f);
                transform.position = Vector3.Lerp(transform.position, playerToFollowAngledDirection, smoothSpeed);
                playerToFollowDirection = new Vector3(player.transform.position.x, player.transform.position.y + 100f, player.transform.position.z);
                minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, playerToFollowDirection, smoothSpeed);
                if (transform.rotation.y != 0) transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(60f, 0f, 0f), 2f);
            }
            else
            {
                playerToFollowAngledDirection = new Vector3(player.transform.position.x + (distanceFromObject / Mathf.Tan(60 * Mathf.PI / 180)) - 0.66f, player.transform.position.y + distanceFromObject, player.transform.position.z);
                transform.position = Vector3.Lerp(transform.position, playerToFollowAngledDirection, smoothSpeed);
                playerToFollowDirection = new Vector3(player.transform.position.x, player.transform.position.y + 100f, player.transform.position.z);
                minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, playerToFollowDirection, smoothSpeed);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(60f, -90f, 0f), 2f);
                // this doesn't work for some reason
                //minimapCamera.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(90f, -90f, 0f), 2.4f);
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
        if (cameraLimiter != null && onXLim)
        {
            if (Mathf.Abs(transform.position.x) > Mathf.Abs(cameraLimiter.xLim))
            {
                xDirection = new Vector3(moveDirection.x + transform.position.x + acceleration, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, xDirection, smoothSpeed);
                if (transform.position.x > 0)
                {
                    if (transform.position.x > cameraLimiter.xLim + 3)
                    {
                        acceleration -= 0.18f;
                    }
                    acceleration -= 0.06f;
                }
                else
                {
                    if (transform.position.x < cameraLimiter.xLim - 3)
                    {
                        acceleration += 0.18f;
                    }
                    acceleration += 0.06f;
                }
            }
            else
            {
                acceleration = 0f;
                onXLim = false;
            }
        }
    }
}