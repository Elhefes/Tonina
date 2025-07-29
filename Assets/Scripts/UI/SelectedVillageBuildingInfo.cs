using UnityEngine;
using TMPro;

public class SelectedVillageBuildingInfo : MonoBehaviour
{
    public string villageBuildingCost;
    public TMP_Text selectedNameTMP;

    private void OnEnable()
    {
        selectedNameTMP.text = gameObject.name + " - " + villageBuildingCost + " Gold";
    }
}
