using UnityEngine;
using TMPro;

public class SelectedVillageBuildingInfo : MonoBehaviour
{
    public int villageBuildingCost;
    public TMP_Text selectedNameTMP;

    private void OnEnable()
    {
        selectedNameTMP.text = gameObject.name + " - " + villageBuildingCost + " Gold";
    }
}
