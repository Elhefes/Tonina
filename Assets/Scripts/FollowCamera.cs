using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform playerCamera;

    private void Awake()
    {
        playerCamera = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (playerCamera != null)
        {
            var rotation = Quaternion.LookRotation(transform.position - playerCamera.transform.position);
            transform.rotation = new Quaternion(rotation.x, 0f, 0f, rotation.w);
        }
    }
}