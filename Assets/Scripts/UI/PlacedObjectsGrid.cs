using UnityEngine;
using TMPro;

public class PlacedObjectsGrid : MonoBehaviour
{
    public GameObject[] placedObjectIndicators;
    public int[] placedObjectAmounts;
    public TMP_Text[] placedObjectIndicatorsTexts;

    public void UpdatePlacedBuildingIndicator(int placedObjectId)
    {
        if (!placedObjectIndicators[placedObjectId].activeSelf && placedObjectAmounts[placedObjectId] > 0)
        {
            placedObjectIndicators[placedObjectId].SetActive(true);
        }

        placedObjectIndicatorsTexts[placedObjectId].text = placedObjectIndicatorsTexts[placedObjectId].name + placedObjectAmounts[placedObjectId].ToString();
        if (placedObjectAmounts[placedObjectId] < 1) placedObjectIndicators[placedObjectId].SetActive(false);
    }

    public void UpdateIndicatorsByArray(int[] existingBuildingAmounts)
    {
        if (existingBuildingAmounts != null)
        {
            //placedObjectAmounts = existingBuildingAmounts; lol this would update the existingBuildingAmounts with placedObjectAmounts??
            System.Array.Copy(existingBuildingAmounts, placedObjectAmounts, existingBuildingAmounts.Length);

            for (int i = 0; i < placedObjectAmounts.Length; i++)
            {
                UpdatePlacedBuildingIndicator(i);
            }
        }
    }

    public bool AnyElementIsActive()
    {
        foreach (int i in placedObjectAmounts)
        {
            if (i > 0) return true;
        }
        return false;
    }
}
