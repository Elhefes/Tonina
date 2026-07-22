using UnityEngine;

public class AttributeUnlockScene : MonoBehaviour
{
    public GameObject overworldUI;
    public GameObject clickBlocker;
    public GameObject firstSlide;

    public Camera playerCamera;
    public GameObject minimapCamera;
    public Camera attributeSceneCamera;

    private bool movingToPlayerCamera;
    private bool onPlayerCamera;

    private float acceleration;

    private void OnEnable()
    {
        movingToPlayerCamera = false;
        onPlayerCamera = false;
        playerCamera.enabled = false;
        attributeSceneCamera.enabled = true;
        attributeSceneCamera.gameObject.SetActive(true);
        minimapCamera.SetActive(false);
        overworldUI.SetActive(false);
        clickBlocker.SetActive(true);
        acceleration = 0f;
        SwitchToAttributeSceneCamera();
    }

    public void ExitScene()
    {
        movingToPlayerCamera = true;
        minimapCamera.SetActive(true);
        overworldUI.SetActive(true);

        playerCamera.gameObject.tag = "MainCamera";
        attributeSceneCamera.gameObject.tag = "Untagged";
    }

    private void SwitchToAttributeSceneCamera()
    {
        attributeSceneCamera.gameObject.tag = "MainCamera";
        attributeSceneCamera.enabled = true;
        playerCamera.gameObject.tag = "Untagged";
        playerCamera.enabled = false;
    }

    private void FixedUpdate()
    {
        if (movingToPlayerCamera && !onPlayerCamera)
        {
            attributeSceneCamera.transform.position = Vector3.Lerp(attributeSceneCamera.transform.position, 
                playerCamera.transform.position, 0.002f + acceleration);
            if (attributeSceneCamera.transform.rotation.y != 0) attributeSceneCamera.transform.rotation
                    = Quaternion.RotateTowards(attributeSceneCamera.transform.rotation, Quaternion.Euler(60f, -90f, 0f), 2f);
            if (Vector3.Distance(attributeSceneCamera.transform.position, playerCamera.transform.position) < 1.5f)
            {
                playerCamera.transform.position = attributeSceneCamera.transform.position;
                onPlayerCamera = true;
                attributeSceneCamera.enabled = false;
                playerCamera.enabled = true;
                clickBlocker.SetActive(false);
                attributeSceneCamera.gameObject.SetActive(false);
                firstSlide.SetActive(true);
                gameObject.SetActive(false);
            }
            acceleration += 0.0008f;
        }
    }
}
