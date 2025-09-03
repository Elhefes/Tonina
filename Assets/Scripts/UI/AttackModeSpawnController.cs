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

    private void OnEnable()
    {
        selectedSpawnNumber = 4;
        UpdateSpawnArray();
        UpdateAddText();
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
