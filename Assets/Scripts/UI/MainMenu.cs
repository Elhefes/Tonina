using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject overworldUI;
    public GameObject clickBlocker;

    public GameObject minimapCamera;
    private Camera playerCamera;
    private Camera mainMenuCamera;

    public Animator optionsBarAnimator;
    public GameObject bar;

    public Animator textsAnimator;
    private Animator mainMenuCameraAnimator;

    private bool movingToPlayerCamera;
    private bool onPlayerCamera;

    private float acceleration;

    public TMP_Text playTimeAmountTMP;
    private int secondsPlayed;

    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        playerCamera = Camera.main;
        mainMenuCamera = GameObject.Find("MainMenuCamera").GetComponent<Camera>();
        mainMenuCameraAnimator = mainMenuCamera.GetComponent<Animator>();

        secondsPlayed = PlayerPrefs.GetInt("secondsPlayed", 0);
        StartCoroutine(SecondCounter());
    }

    private void Awake()
    {
        StartCoroutine(SwitchMainCameraToMainMenuCamera());
    }

    public void ContinueHere()
    {
        if (PlayerPrefs.GetInt("introPlayed", 0) == 0)
        {
            textsAnimator.SetTrigger("PlayIntroFirstText");
            return;
        }
        if (bar.transform.localPosition.x > -1000f)
        {
            CloseOptions();
            Invoke("ExitMainMenuView", 0.75f); // Wait for options bar to close
            return;
        }
        ExitMainMenuView();
    }

    void ExitMainMenuView()
    {
        mainMenuCameraAnimator.enabled = false;
        movingToPlayerCamera = true;
        mainMenuUI.SetActive(false);
        minimapCamera.SetActive(true);
        overworldUI.SetActive(true);

        playerCamera.gameObject.tag = "MainCamera";
        mainMenuCamera.gameObject.tag = "Untagged";
    }

    public void StartEnteringMainMenu()
    {
        Invoke("EnterMainMenuView", 0.33f);
    }

    void EnterMainMenuView()
    {
        mainMenuCameraAnimator.enabled = true;
        movingToPlayerCamera = false;
        onPlayerCamera = false;
        minimapCamera.SetActive(false);
        mainMenuUI.SetActive(true);
        overworldUI.SetActive(false);
        clickBlocker.SetActive(true);
        acceleration = 0f;

        StartCoroutine(SwitchMainCameraToMainMenuCamera());
    }

    public void PlayAsAttackers()
    {
        textsAnimator.SetTrigger("GrandiosityText");
    }

    public void OpenOptions()
    {
        optionsBarAnimator.SetTrigger("Open");
    }

    public void CloseOptions()
    {
        optionsBarAnimator.SetTrigger("Close");
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

    IEnumerator SwitchMainCameraToMainMenuCamera()
    {
        yield return new WaitForEndOfFrame();
        mainMenuCamera.gameObject.tag = "MainCamera";
        mainMenuCamera.enabled = true;
        playerCamera.gameObject.tag = "Untagged";
        playerCamera.enabled = false;
    }

    IEnumerator SecondCounter()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            secondsPlayed++;
            PlayerPrefs.SetInt("secondsPlayed", secondsPlayed);
            TimeSpan time = TimeSpan.FromSeconds(secondsPlayed);

            string timePlayed = time.TotalHours.ToString("00") + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
            playTimeAmountTMP.text = timePlayed;
        }
    }
}
