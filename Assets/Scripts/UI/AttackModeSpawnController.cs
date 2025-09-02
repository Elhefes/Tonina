using UnityEngine;
using TMPro;

public class AttackModeSpawnController : MonoBehaviour
{
    public int[] spawnArray;
    private int selectedSpawnNumber;
    public TMP_Text[] spawnTexts;
    public GameObject amountButtons;

    private void OnEnable()
    {
        selectedSpawnNumber = 4;
        UpdateSpawnArray();
    }

    public void IncreaseSpawn()
    {
        spawnArray[selectedSpawnNumber]++;
        UpdateSpawnArray();
    }

    public void DecreaseSpawn()
    {
        if (spawnArray[selectedSpawnNumber] > 0) spawnArray[selectedSpawnNumber]--;
        UpdateSpawnArray();
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
}
