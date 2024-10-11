using UnityEngine;

public class CustomWeaponOrder : MonoBehaviour
{
    // When bow is added, place it to the second last slot

    private string customWeaponOrder;
    private int[] slotXValues = { -500, -250, 0, 500 };
    private int chosenWepIndex = 2;
    public Transform moveButtons;
    public Transform clubSlot;
    public Transform spearSlot;
    public Transform axeSlot;
    public Transform smallStoneSlot;

    private void Start()
    {
        customWeaponOrder = PlayerPrefs.GetString("CustomWeaponOrder", "0123");
        UpdateSlotPositions();
    }

    public void UpdateChosenWepIndex(Transform slot)
    {
        if (slot.transform.position.x - 960 == -500) chosenWepIndex = 0;
        else if (slot.transform.position.x - 960 == -250) chosenWepIndex = 1;
        else if (slot.transform.position.x - 960 == 0) chosenWepIndex = 2;
        else if (slot.transform.position.x - 960 == 500) chosenWepIndex = 3;
        UpdateMoveButtonsPosition();
    }

    public void UpdateMoveButtonsPosition()
    {
        moveButtons.position = new Vector3(slotXValues[chosenWepIndex] + 960, 540, 0);
    }

    public void ResetWeaponOrder()
    {
        PlayerPrefs.SetString("CustomWeaponOrder", "0123");
        customWeaponOrder = "0123";
        UpdateSlotPositions();
    }

    private void UpdateSlotPositions()
    {
        for (int i = 0; i < customWeaponOrder.Length; i++)
        {
            if (customWeaponOrder[i].ToString() == "0") {
                clubSlot.position = new Vector3(slotXValues[i] + 960, 540, 0);
            }
            else if (customWeaponOrder[i].ToString() == "1")
            {
                spearSlot.position = new Vector3(slotXValues[i] + 960, 540, 0);
            }
            else if (customWeaponOrder[i].ToString() == "2")
            {
                axeSlot.position = new Vector3(slotXValues[i] + 960, 540, 0);
            }
            else if (customWeaponOrder[i].ToString() == "3")
            {
                smallStoneSlot.position = new Vector3(slotXValues[i] + 960, 540, 0);
            }
        }
    }

    public void MoveDigitToRight()
    {
        // Check if n is within valid bounds
        if (chosenWepIndex < 3)
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
