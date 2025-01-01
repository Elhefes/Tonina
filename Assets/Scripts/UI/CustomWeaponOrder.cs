using UnityEngine;

public class CustomWeaponOrder : MonoBehaviour
{
    private string customWeaponOrder;
    private int[] slotXValues = { -500, -250, 0, 250, 500 };
    private int chosenWepIndex = 2;
    public Transform moveButtons;
    public Transform clubSlot;
    public Transform spearSlot;
    public Transform axeSlot;
    public Transform bowSlot;
    public Transform smallStoneSlot;

    private void Start()
    {
        customWeaponOrder = PlayerPrefs.GetString("CustomWeaponOrder", "01234");
        UpdateSlotPositions();
    }

    public void UpdateChosenWepIndex(Transform slot)
    {
        if (slot.transform.localPosition.x == -500) chosenWepIndex = 0;
        else if (slot.transform.localPosition.x == -250) chosenWepIndex = 1;
        else if (slot.transform.localPosition.x == 0) chosenWepIndex = 2;
        else if (slot.transform.localPosition.x == 250) chosenWepIndex = 3;
        else if (slot.transform.localPosition.x == 500) chosenWepIndex = 4;
        UpdateMoveButtonsPosition();
    }

    public void UpdateMoveButtonsPosition()
    {
        moveButtons.localPosition = new Vector3(slotXValues[chosenWepIndex], 0, 0);
    }

    public void ResetWeaponOrder()
    {
        PlayerPrefs.SetString("CustomWeaponOrder", "01234");
        customWeaponOrder = "01234";
        UpdateSlotPositions();
    }

    private void UpdateSlotPositions()
    {
        for (int i = 0; i < customWeaponOrder.Length; i++)
        {
            if (customWeaponOrder[i].ToString() == "0") {
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

    public void MoveDigitToRight()
    {
        // Check if n is within valid bounds
        if (chosenWepIndex < 4)
        {
            // Extract the nth digit
            char digitToMove = customWeaponOrder[chosenWepIndex]; // n-1 for 0-based index

            // Remove the nth digit
            string numberWithoutDigit = customWeaponOrder.Remove(chosenWepIndex, 1);

            // Insert the digit at the next position
            string resultString = numberWithoutDigit.Insert(chosenWepIndex + 1, digitToMove.ToString());
            PlayerPrefs.SetString("CustomWeaponOrder", resultString);
            customWeaponOrder = resultString;
            chosenWepIndex++;
            UpdateSlotPositions();
            UpdateMoveButtonsPosition();
        }
    }

    public void MoveDigitToLeft()
    {
        // Check if n is within valid bounds
        if (chosenWepIndex > 0)
        {
            // Extract the nth digit
            char digitToMove = customWeaponOrder[chosenWepIndex];

            // Remove the nth digit
            string numberWithoutDigit = customWeaponOrder.Remove(chosenWepIndex, 1);

            // Insert the digit at the previous position
            string resultString = numberWithoutDigit.Insert(chosenWepIndex - 1, digitToMove.ToString());
            PlayerPrefs.SetString("CustomWeaponOrder", resultString);
            customWeaponOrder = resultString;
            chosenWepIndex--;
            UpdateSlotPositions();
            UpdateMoveButtonsPosition();
        }
    }
}
