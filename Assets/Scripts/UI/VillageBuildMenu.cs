using UnityEngine;
using UnityEngine.EventSystems;

public class VillageBuildMenu : MonoBehaviour, IPointerDownHandler
{
    public GameObject defaultTexts;
    public GameObject buyButtonObject;
    public bool buildingSelected;
    public VillageBuildSelection[] villageBuildSelections;

    public void UnselectAll()
    {
        if (villageBuildSelections != null)
        {
            foreach (VillageBuildSelection selection in villageBuildSelections)
            {
                if (selection.selectedVillageBuildingInfo.gameObject.activeSelf)
                {
                    selection.highlighted = false;
                    selection.selectedVillageBuildingInfo.gameObject.SetActive(false);
                }
            }
        }
        buyButtonObject.SetActive(false);
        defaultTexts.SetActive(true);
        buildingSelected = false;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        UnselectAll();
    }
}
