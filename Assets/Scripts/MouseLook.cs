using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float moveSpeed;
    public float sensitivity;
    public LayerMask layerMask;
    private Vector3 moveDirection;
    private Vector3 playerToFollowDirection;
    public float distanceFromObject;
    private float currentDistance;
    public float smoothSpeed;
    public float minCameraZoom;
    public float maxCameraZoom;
    public GameObject playerToFollow;
    public bool cameraOnPlayer = true;

    // Update is called once per frame
    void Update()
    {
        if (!cameraOnPlayer)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        // Use scroll wheel to zoom in or out
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        distanceFromObject = Mathf.Clamp(distanceFromObject - scrollWheel * sensitivity, minCameraZoom, maxCameraZoom);

        if (cameraOnPlayer)
        {
            playerToFollowDirection = new Vector3(playerToFollow.transform.position.x, playerToFollow.transform.position.y + distanceFromObject, playerToFollow.transform.position.z - (distanceFromObject / Mathf.Tan(60 * Mathf.PI / 180)) + 0.66f);
            transform.position = Vector3.Lerp(transform.position, playerToFollowDirection, smoothSpeed);
        }
        else
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