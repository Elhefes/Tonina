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
    public TMP_Text addText;
    public TMP_Text[] spawnTexts;
    public GameObject amountButtons;
    public GameObject startButton;

    public AttackModeCreatureSpawner attackModeCreatureSpawner;
    public GameObject enemiesObject;

    public GameObject playerSpawnElement;
    public GameObject spawnTextInBetween;
    public GameObject leftestSpawnText;
    public GameObject rightestSpawnText;

    public GameObject battleUI;

    private void OnEnable()
    {
        playerSpawnNumber = 4;
        selectedSpawnNumber = 4;
        UpdateSpawnArray();
        UpdateAddText();
        UpdatePlayerSpawnElements(playerSpawnNumber, 0f);
        animator.SetTrigger("Up");
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
        playerSpawnElement.transform.localPosition = new Vector3(buttonPosition, -490f, 0f);
    }

    public void StartAttack()
    {
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
        battleUI.SetActive(true);
        mouseLook.ToggleCameraOnPlayer();
        enemiesObject.SetActive(true);
        attackModeCreatureSpawner.MoveFriendliesToSpawns(spawnArray);
        attackModeCreatureSpawner.friendliesParentObject.SetActive(true);
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
