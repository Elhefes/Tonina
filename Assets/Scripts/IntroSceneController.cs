using System.Collections;
using UnityEngine;

public class IntroSceneController : MonoBehaviour
{
    public GameObject clickBlocker;

    public GameObject sartomIntroObject;
    public Player playerPivot;
    public GameObject staticTwinStatue;

    public Camera playerCamera;
    public Camera introCamera;
    private Animator introCameraAnimator;
    private bool onPlayerCamera;
    private bool switchPresented;
    private float acceleration;

    public Enemy[] introEnemies;

    public GameObject spearPickUpPointerObject;

    public IntroBlackFader introBlackFader;

    public IntroHUD_Controller introHUD_Controller;

    public WeaponWheel weaponWheel;

    private void OnEnable()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        introCameraAnimator = introCamera.GetComponent<Animator>();
        if (introCameraAnimator != null) introCameraAnimator.SetBool("FirstSceneFinished", false);
    }

    public void EnableOnlyBattleModeObjects()
    {
        playerPivot.gameObject.SetActive(true);
        playerPivot.EnableBattleMode();
        staticTwinStatue.SetActive(true);
        sartomIntroObject.SetActive(false);
    }

    public void TryToMoveToNextEvent(int eventIndex)
    {
        if (eventIndex == 0)
        {
            if (introEnemies[0].healthBar.value <= 0 && introEnemies[1].healthBar.value <= 0) // 1st wave is cleared
            {
                introHUD_Controller.optionsMenu.gameObject.SetActive(false);
                introHUD_Controller.presenting = true;
                introHUD_Controller.presentationStartObjects[1].SetActive(true);
            }
        }
        else if (eventIndex == 1)
        {
            if (usableWeaponsAmount() > 1) // Start weapon switching presentation
            {
                introHUD_Controller.optionsMenu.gameObject.SetActive(false);
                introHUD_Controller.presentationStartObjects[2].SetActive(true);
            }
            else if (!playerPivot.spear.selected) StartSpearPickUpPresentation();
            else SendLastEnemies();
        }
        else if (eventIndex == 2)
        {
            if (!playerPivot.spear.selected) StartSpearPickUpPresentation();
            else FinishGameplaySection();
        }
        else
        {
            if (introEnemies[3].healthBar.value <= 0 && introEnemies[4].healthBar.value <= 0) // Last wave is cleared
            {
                FinishGameplaySection();
            }
        }
    }

    public void SendFirstEnemies()
    {
        introHUD_Controller.presenting = false;
        StartCoroutine(FirstEnemyWave());
    }

    void SendLastEnemies()
    {
        introEnemies[3].gameObject.SetActive(true);
        introEnemies[4].gameObject.SetActive(true);
    }

    IEnumerator FirstEnemyWave()
    {
        yield return new WaitForSeconds(2.5f);
        introEnemies[0].creatureMovement.agent.speed = 3.5f; // from 0 to default speed, i.e. "starts to move"
        yield return new WaitForSeconds(8.5f);
        introEnemies[1].gameObject.SetActive(true);
    }

    void StartSpearPickUpPresentation()
    {
        if (introHUD_Controller.presentationStartObjects[3] != null) introHUD_Controller.presentationStartObjects[3].SetActive(true);
        spearPickUpPointerObject.SetActive(true);
        playerPivot.spear.notAvailable = true;
        playerPivot.spear.quantity = 0;
        weaponWheel.AddWeaponToSelectedWeapons(1); // 1 = Spear's weapon ID
    }

    void FinishGameplaySection()
    {
        spearPickUpPointerObject.SetActive(false);

        // Don't stay in an aiming animation
        if (playerPivot.weaponOnHand.name == "Spear" || playerPivot.weaponOnHand.name == "Small Stone")
        {
            playerPivot.creatureMovement.animator.SetInteger("WeaponIndex", 0);
        }

        introHUD_Controller.battleUI.SetActive(false);
        introHUD_Controller.optionsMenu.gameObject.SetActive(false);
        introHUD_Controller.presenting = true;

        introBlackFader.gameObject.SetActive(true);
        introBlackFader.animator.SetTrigger("Fade1");
    }

    public void StartLastScene()
    {
        introCameraAnimator.enabled = true;
        introCameraAnimator.SetTrigger("LastScene");
        introCamera.enabled = true;
    }

    public void StartFinalFade()
    {
        introBlackFader.gameObject.SetActive(true);
        introBlackFader.animator.SetTrigger("Fade2");
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
                    introHUD_Controller.EnableBattleModeHUD(true);
                }
                acceleration += 0.0008f;
            }
        }
        if (introHUD_Controller.presentationStartObjects[2].activeSelf) // Weapon switch presentation
        {
            if (playerPivot.weaponRangeIndicatorLight.intensity > 1f) // If weapon is switched (fast clicker pros are allowed to skip lol)
            {
                introHUD_Controller.presentationStartObjects[2].SetActive(false);
                switchPresented = true;
                introHUD_Controller.presenting = false;
                introEnemies[2].gameObject.SetActive(true);
            }
        }
        else if (introHUD_Controller.presentationStartObjects[3].activeSelf) // Spear pick-up presentation
        {
            if (playerPivot.spear.quantity > 0)
            {
                introHUD_Controller.pickUpSpearTextBox.SetActive(false);
                EnableEquipSpearTextBox();

                if ((introHUD_Controller.equipSpearTextBoxSwitchedVersion.activeSelf ||
                    introHUD_Controller.equipSpearTextBoxNotSwitchedVersion.activeSelf) && playerPivot.weaponOnHand.name == "Spear")
                {
                    introHUD_Controller.presentationStartObjects[3].SetActive(false);
                    SendLastEnemies();
                }
            }
        }
    }

     private void EnableEquipSpearTextBox()
    {
        if (switchPresented) introHUD_Controller.equipSpearTextBoxSwitchedVersion.SetActive(true);
        else introHUD_Controller.equipSpearTextBoxNotSwitchedVersion.SetActive(true);
    }

    private int usableWeaponsAmount()
    {
        int i = 0;
        foreach (Weapon wep in playerPivot.weapons)
        {
            if (!wep.notAvailable && wep.selected) i++;
        }
        return i;
    }
}
