using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform playerCamera;

    private void Awake()
    {
        if (playerCamera == null)
        {
            if (Camera.main != null) playerCamera = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (playerCamera != null)
        {
            var rotation = Quaternion.LookRotation(transform.position - playerCamera.transform.position);
            transform.rotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
        }
    }
}