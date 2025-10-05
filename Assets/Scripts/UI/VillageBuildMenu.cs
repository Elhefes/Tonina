using UnityEngine;
using UnityEngine.EventSystems;

public class VillageBuildMenu : MonoBehaviour, IPointerDownHandler
{
    public GameObject defaultTexts;
    public GameObject buyButtonObject;
    public bool buildingSelected;
    public VillageBuildSelection[] villageBuildSelections;
    public VillageBuildSelection currentBuildSelection;

    public ToninaCutSceneCamera toninaCutSceneCamera;

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
        currentBuildSelection = null;
        defaultTexts.SetActive(true);
        buildingSelected = false;
    }

    public void BuyCurrentSelection()
    {
        if (currentBuildSelection != null)
        {
            currentBuildSelection.BuyThisSelection();
            UnselectAll();
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        UnselectAll();
    }
}
