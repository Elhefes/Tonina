using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelection : MonoBehaviour
{
    public int maxWeaponAmount; // Default = 3, add amulet that increases to 4
    private int selectedWeaponsAmount;

    private string customWeaponOrder;
    private int[] slotXValues = { -500, -250, 0, 250, 500 };
    public Transform clubSlot;
    public Transform spearSlot;
    public Transform axeSlot;
    public Transform bowSlot;
    public Transform smallStoneSlot;

    public TMP_Text selectMoreTMP;

    [Header("Images & Sprites")]
    public Image[] spriteArray;
    public Sprite weaponSelectedSprite;
    public Sprite weaponNotSelectedSprite;

    private void OnEnable()
    {
        //selectedWeaponsAmount = 0;
        maxWeaponAmount = 3;
        customWeaponOrder = PlayerPrefs.GetString("CustomWeaponOrder", "01234");
        UpdateSlotPositions();
        UpdateSelectionText();
    }

    public void SelectWeaponIfPossible(int weaponIndex)
    {
        if (spriteArray[weaponIndex].sprite == weaponNotSelectedSprite)
        {
            selectedWeaponsAmount++;
            spriteArray[weaponIndex].sprite = weaponSelectedSprite;
            UpdateSelectionText();
        }
        else
        {
            selectedWeaponsAmount--;
            spriteArray[weaponIndex].sprite = weaponNotSelectedSprite;
            UpdateSelectionText();
        }
    }

    void UpdateSelectionText()
    {
        selectMoreTMP.text = "Choose " + (maxWeaponAmount - selectedWeaponsAmount) + " more!";
    }

    private void UpdateSlotPositions()
    {
        for (int i = 0; i < customWeaponOrder.Length; i++)
        {
            if (customWeaponOrder[i].ToString() == "0")
            {
                clubSlot.localPosition = new Vector3(slotXValues[i], 0, 0);
            }
            else if (customWeaponOrder[i].ToString() == "1")
            {
                spearSlot.localPosition = new Vector3(slotXValues[i], 0, 0);
            }
            else if (customWeaponOrder[i].ToString() == "2")
            {
                axeSlot.localPosition = new Vector3(slotXValues[i], 0, 0);
            }
            else if (customWeaponOrder[i].ToString() == "3")
            {
                bowSlot.localPosition = new Vector3(slotXValues[i], 0, 0);
            }
            else if (customWeaponOrder[i].ToString() == "4")
            {
                smallStoneSlot.localPosition = new Vector3(slotXValues[i], 0, 0);
            }
        }
    }
}
