using UnityEngine;
using TMPro;

public class AttackModeSpawnController : MonoBehaviour
{
    public MouseLook mouseLook;
    public Camera mainCamera;
    public AudioListener minimapAudioListener;
    public RenderTexture minimapRenderTexture;
    public GameObject minimapRenderTextureObject;

    public Animator animator;
    public Transform moveableElements;
    private bool isDown;

    public int maxFriendliesAmount;
    private int currentFriendliesAmount;
    public int[] spawnArray;
    private int playerSpawnNumber;
    private int selectedSpawnNumber;
    private float playerSpawnButtonPosition;
    public TMP_Text addText;
    public TMP_Text[] spawnTexts;
    public GameObject amountButtons;
    public GameObject startButton;

    public AttackModeCreatureSpawner attackModeCreatureSpawner;

    public GameObject playerSpawnElement;
    public GameObject spawnTextInBetween;
    public GameObject leftestSpawnText;
    public GameObject rightestSpawnText;

    public GameObject battleUI;
    public GameObject optionsButton;

    private void Start()
    {
        playerSpawnNumber = 4;
        selectedSpawnNumber = 4;
        playerSpawnButtonPosition = 0f;
        UpdatePlayerSpawnElements(playerSpawnNumber, playerSpawnButtonPosition); // Start updates later than OnEnable
        animator.SetTrigger("Up");
    }

    private void OnEnable()
    {
        ResetSpawns();
        optionsButton.SetActive(true);
    }

    void ResetSpawns()
    {
        System.Array.Clear(spawnArray, 0, spawnArray.Length);
        currentFriendliesAmount = 0;
        isDown = false;
        UpdateSpawnArray();
        UpdateAddText();
        UpdatePlayerSpawnElements(playerSpawnNumber, playerSpawnButtonPosition);
        startButton.SetActive(false);
        amountButtons.SetActive(true);
    }

    public void UpDownTrigger()
    {
        if (Mathf.Abs(moveableElements.localPosition.y) > 0.5f && Mathf.Abs(moveableElements.localPosition.y + 330) > 0.5f) return; // When elements aren't up or down
        isDown = !isDown;
        if (isDown)
        {
            animator.SetTrigger("Down");
            amountButtons.SetActive(false);
        }
        else
        {
            animator.SetTrigger("Up");
            amountButtons.SetActive(true);
        }
    }

    public void IncreaseSpawn()
    {
        if (currentFriendliesAmount < maxFriendliesAmount)
        {
            spawnArray[selectedSpawnNumber]++;
            currentFriendliesAmount++;
            UpdateSpawnArray();
            UpdateAddText();

            if (currentFriendliesAmount == maxFriendliesAmount)
            {
                startButton.SetActive(true);
                UpDownTrigger();
            }
        }
    }

    public void DecreaseSpawn()
    {
        if (spawnArray[selectedSpawnNumber] > 0)
        {
            spawnArray[selectedSpawnNumber]--;
            currentFriendliesAmount--;
            UpdateSpawnArray();
            UpdateAddText();
            startButton.SetActive(false);
        }
    }

    public void UpdateSelectedSpawnNumber(int spawnNumber)
    {
        if (!isDown) selectedSpawnNumber = spawnNumber;
    }

    public void UpdateButtonsPosition(float elementPositionX)
    {
        if (!isDown) amountButtons.transform.localPosition = new Vector3(elementPositionX, 0f, 0f);
    }

    public void UpdatePlayerSpawnElements(int spawnNumber, float buttonPosition)
    {
        if (isDown) return;

        if (spawnNumber == 0)
        {
            leftestSpawnText.SetActive(true);
            rightestSpawnText.SetActive(false);
            spawnTextInBetween.SetActive(false);
        }
        else if (spawnNumber == 8)
        {
            leftestSpawnText.SetActive(false);
            rightestSpawnText.SetActive(true);
            spawnTextInBetween.SetActive(false);
        }
        else
        {
            leftestSpawnText.SetActive(false);
            rightestSpawnText.SetActive(false);
            spawnTextInBetween.SetActive(true);
        }

        playerSpawnNumber = spawnNumber;
        playerSpawnButtonPosition = buttonPosition;
        playerSpawnElement.transform.localPosition = new Vector3(playerSpawnButtonPosition, -490f, 0f);
    }

    public void StartAttack()
    {
        mouseLook.SetMovingToSpecificPosition(false);
        mainCamera.gameObject.tag = "MainCamera";
        mainCamera.enabled = true;
        minimapAudioListener.enabled = false;
        mouseLook.minimapCamera.gameObject.tag = "Untagged";
        mouseLook.minimapCamera.targetTexture = minimapRenderTexture;
        mouseLook.notCastingRays = false;
        mouseLook.player.transform.position = attackModeCreatureSpawner.spawns[playerSpawnNumber].position;
        mouseLook.player.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(true);
        minimapRenderTextureObject.SetActive(true);
        optionsButton.SetActive(false);
        battleUI.SetActive(true);
        mouseLook.ToggleCameraOnPlayer();
        attackModeCreatureSpawner.SetEnemiesActive(true);
        attackModeCreatureSpawner.MoveFriendliesToSpawns(spawnArray);
        attackModeCreatureSpawner.SetFriendliesActive(true);
        gameObject.SetActive(false); // This gameObject is the SelectAttackPoints UI object
    }

    public void ReturnToSpawnSelection()
    {
        mainCamera.gameObject.tag = "Untagged";
        mainCamera.enabled = false;
        minimapAudioListener.enabled = true;
        mouseLook.minimapCamera.gameObject.tag = "MainCamera";
        mouseLook.minimapCamera.targetTexture = null;
        if (mouseLook.cameraOnPlayer) mouseLook.ToggleCameraOnPlayer();
        mouseLook.notCastingRays = true;
        mouseLook.player.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        minimapRenderTextureObject.SetActive(false);
        battleUI.SetActive(false);
        attackModeCreatureSpawner.SetEnemiesActive(false);
        attackModeCreatureSpawner.SetFriendliesActive(false);
        gameObject.SetActive(true); // This gameObject is the SelectAttackPoints UI object
    }

    void UpdateSpawnArray()
    {
        if (isDown) return;
        for (int i = 0; i < spawnArray.Length; i++)
        {
            spawnTexts[i].text = spawnArray[i].ToString();
        }
    }

    void UpdateAddText()
    {
        if (isDown) return;
        if (currentFriendliesAmount == maxFriendliesAmount)
        {
            addText.text = "All spawns selected!";
        }
        else
        {
            addText.text = "TT Warrior spawns:\nAdd " + (maxFriendliesAmount - currentFriendliesAmount) + " more!";
        }
    }
}
