using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject overworldUI;
    public GameObject clickBlocker;

    private Camera playerCamera;
    private Camera mainMenuCamera;

    public Animator textsAnimator;
    private Animator mainMenuCameraAnimator;

    private bool movingToPlayerCamera;
    private bool onPlayerCamera;

    private float acceleration;

    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        playerCamera = Camera.main;
        mainMenuCamera = GameObject.Find("MainMenuCamera").GetComponent<Camera>();
        mainMenuCameraAnimator = mainMenuCamera.GetComponent<Animator>();
    }

    public void SetIntroAsPlayed()
    {
        PlayerPrefs.SetInt("introPlayed", 1); // Set this later to be called when intro is finished, not when it's started
    }

    public void ContinueHere()
    {
        if (PlayerPrefs.GetInt("introPlayed", 0) == 0)
        {
            textsAnimator.SetTrigger("PlayIntroFirstText");
            return;
        }
        mainMenuCameraAnimator.enabled = false;
        movingToPlayerCamera = true;
        mainMenuUI.SetActive(false);
        overworldUI.SetActive(true);
    }

    public void PlayAsAttackers()
    {
        textsAnimator.SetTrigger("GrandiosityText");
    }

    private void FixedUpdate()
    {
        if (movingToPlayerCamera && !onPlayerCamera)
        {
            mainMenuCamera.transform.position = Vector3.Lerp(mainMenuCamera.transform.position, playerCamera.transform.position, 0.002f + acceleration);
            if (mainMenuCamera.transform.rotation.y != 0) mainMenuCamera.transform.rotation = Quaternion.RotateTowards(mainMenuCamera.transform.rotation, Quaternion.Euler(60f, -90f, 0f), 2f);
            if (Vector3.Distance(mainMenuCamera.transform.position, playerCamera.transform.position) < 1.5f)
            {
                playerCamera.transform.position = mainMenuCamera.transform.position;
                onPlayerCamera = true;
                mainMenuCamera.enabled = false;
                playerCamera.enabled = true;
                clickBlocker.SetActive(false);
            }
            acceleration += 0.0008f;
        }
    }
}
