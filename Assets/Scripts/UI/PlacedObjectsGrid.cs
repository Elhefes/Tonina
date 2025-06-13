using UnityEngine;
using TMPro;

public class PlacedObjectsGrid : MonoBehaviour
{
    public GameObject[] placedObjectIndicators;
    public int[] placedObjectAmounts;
    public TMP_Text[] placedObjectIndicatorsTexts;

    public void UpdatePlacedBuildingIndicator(int placedObjectId, int amount)
    {
        placedObjectAmounts[placedObjectId] += amount;

        if (!placedObjectIndicators[placedObjectId].activeSelf && placedObjectAmounts[placedObjectId] > 0)
        {
            placedObjectIndicators[placedObjectId].SetActive(true);
        }

        placedObjectIndicatorsTexts[placedObjectId].text = placedObjectIndicatorsTexts[placedObjectId].name + placedObjectAmounts[placedObjectId].ToString();
        if (placedObjectAmounts[placedObjectId] < 1) placedObjectIndicators[placedObjectId].SetActive(false);
    }
}
