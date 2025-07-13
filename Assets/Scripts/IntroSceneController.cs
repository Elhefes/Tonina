using UnityEngine;

public class IntroSceneController : MonoBehaviour
{
    public GameObject clickBlocker;

    public GameObject sartomIntroObject;
    public GameObject playerPivot;
    public GameObject staticTwinStatue;

    public Camera playerCamera;
    public Camera introCamera;
    private Animator introCameraAnimator;
    private bool onPlayerCamera;
    private float acceleration;

    private void OnEnable()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        introCameraAnimator = introCamera.GetComponent<Animator>();
        if (introCameraAnimator != null) introCameraAnimator.SetBool("FirstSceneFinished", false);
    }

    public void EnableOnlyBattleModeObjects()
    {
        playerPivot.SetActive(true);
        staticTwinStatue.SetActive(true);
        sartomIntroObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!onPlayerCamera)
        {
            if (introCameraAnimator.GetBool("FirstSceneFinished"))
            {
                introCameraAnimator.enabled = false;
                introCamera.transform.position = Vector3.Lerp(introCamera.transform.position, playerCamera.transform.position, 0.002f + acceleration);
                if (introCamera.transform.rotation.y != 0) introCamera.transform.rotation = Quaternion.RotateTowards(introCamera.transform.rotation, Quaternion.Euler(60f, -90f, 0f), 2f);
                if (Vector3.Distance(introCamera.transform.position, playerCamera.transform.position) < 0.2f)
                {
                    playerCamera.transform.position = introCamera.transform.position;
                    onPlayerCamera = true;
                    introCamera.enabled = false;
                    playerCamera.enabled = true;
                    clickBlocker.SetActive(false);
                }
                acceleration += 0.0008f;
            }
        }
    }
}
