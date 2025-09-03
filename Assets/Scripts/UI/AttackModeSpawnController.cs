using UnityEngine;
using TMPro;

public class AttackModeSpawnController : MonoBehaviour
{
    public int maxFriendliesAmount;
    private int currentFriendliesAmount;
    public int[] spawnArray;
    private int selectedSpawnNumber;
    public TMP_Text addText;
    public TMP_Text[] spawnTexts;
    public GameObject amountButtons;

    public GameObject playerSpawnElement;
    public GameObject spawnTextInBetween;
    public GameObject leftestSpawnText;
    public GameObject rightestSpawnText;

    private void OnEnable()
    {
        selectedSpawnNumber = 4;
        UpdateSpawnArray();
        UpdateAddText();
        UpdatePlayerSpawnElements(4, 0f);
    }

    public void IncreaseSpawn()
    {
        if (currentFriendliesAmount < maxFriendliesAmount)
        {
            spawnArray[selectedSpawnNumber]++;
            currentFriendliesAmount++;
            UpdateSpawnArray();
            UpdateAddText();
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
        }
    }

    public void UpdateSelectedSpawnNumber(int spawnNumber)
    {
        selectedSpawnNumber = spawnNumber;
    }

    public void UpdateButtonsPosition(float elementPositionX)
    {
        amountButtons.transform.localPosition = new Vector3(elementPositionX, 0f, 0f);
    }

    public void UpdatePlayerSpawnElements(int spawnNumber, float buttonPosition)
    {
        if (spawnNumber == 0)
        {
            leftestSpawnText.SetActive(true);
            spawnTextInBetween.SetActive(false);
        }
        else if (spawnNumber == 8)
        {
            rightestSpawnText.SetActive(true);
            spawnTextInBetween.SetActive(false);
        }
        else
        {
            leftestSpawnText.SetActive(false);
            rightestSpawnText.SetActive(false);
            spawnTextInBetween.SetActive(true);
        }

        playerSpawnElement.transform.localPosition = new Vector3(buttonPosition, -490f, 0f);
    }

    void UpdateSpawnArray()
    {
        for (int i = 0; i < spawnArray.Length; i++)
        {
            spawnTexts[i].text = spawnArray[i].ToString();
        }
    }

    void UpdateAddText()
    {
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
