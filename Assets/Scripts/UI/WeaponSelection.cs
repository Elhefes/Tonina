using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponSelection : MonoBehaviour
{
    public int maxWeaponAmount; // Default = 3, add amulet that increases to 4
    private int selectedWeaponsAmount;

    private string customWeaponOrder;
    private string selectedWeaponOrder;
    private int[] slotXValues = { -500, -250, 0, 250, 500 };
    public Transform clubSlot;
    public Transform spearSlot;
    public Transform axeSlot;
    public Transform bowSlot;
    public Transform smallStoneSlot;

    public TMP_Text selectMoreTMP;

    public Animator meleeTextAnimator;

    [Header("Images & Sprites")]
    public Image[] spriteArray;
    public Sprite weaponSelectedSprite;
    public Sprite weaponNotSelectedSprite;

    private void OnEnable()
    {
        meleeTextAnimator.SetTrigger("Reset");
        maxWeaponAmount = 3;
        customWeaponOrder = PlayerPrefs.GetString("CustomWeaponOrder", "01234");
        selectedWeaponOrder = PlayerPrefs.GetString("SelectedWeaponOrder", "0");

        SelectWeaponsFromString();
        selectedWeaponsAmount = selectedWeaponOrder.Length;
        UpdateSelectionText();
        UpdateSlotPositions();
    }

    public void SelectWeaponIfPossible(int weaponIndex)
    {
        if (spriteArray[weaponIndex].sprite == weaponNotSelectedSprite)
        {
            if (selectedWeaponsAmount < maxWeaponAmount)
            {
                SelectWeapon(weaponIndex);
            }
        }
        else
        {
            if ((weaponIndex == 0 && !selectedWeaponOrder.Contains("2")) || 
                (weaponIndex == 2 && !selectedWeaponOrder.Contains("0"))) // 1 Melee weapon must be selected always
            {
                meleeTextAnimator.SetTrigger("MeleePopUp");
                return;
            }

            selectedWeaponsAmount--;
            spriteArray[weaponIndex].sprite = weaponNotSelectedSprite;
            RemoveSelectedFromString(weaponIndex);
            UpdateSelectionText();
            UpdateSelectedWeaponOrder();
        }
    }

    void SelectWeapon(int weaponIndex)
    {
        spriteArray[weaponIndex].sprite = weaponSelectedSprite;
        if (!selectedWeaponOrder.Contains(weaponIndex.ToString()))
        {
            AddSelectedToString(weaponIndex);
            selectedWeaponsAmount++;
        }
        UpdateSelectionText();
        UpdateSelectedWeaponOrder();
    }

    void AddSelectedToString(int weaponIndex)
    {
        selectedWeaponOrder += weaponIndex;
    }

    void RemoveSelectedFromString(int weaponIndex)
    {
        selectedWeaponOrder = selectedWeaponOrder.Replace(weaponIndex.ToString(), "");
    }

    void UpdateSelectedWeaponOrder()
    {
        PlayerPrefs.SetString("SelectedWeaponOrder", selectedWeaponOrder);
    }

    public void SelectWeaponsFromString()
    {
        List<int> intList = ConvertStringToIntArray(selectedWeaponOrder);
        foreach (int i in intList)
        {
            SelectWeapon(i);
        }
    }

    public List<int> ConvertStringToIntArray(string input)
    {
        List<int> intList = new List<int>();

        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                intList.Add(int.Parse(c.ToString()));
            }
        }
        return intList;
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
