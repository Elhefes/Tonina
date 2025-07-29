using UnityEngine;
using UnityEngine.EventSystems;

public class VillageBuildMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public bool buildingSelected;
    public VillageBuildSelection villageBuildSelection;
    public GameObject defaultTexts;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData pointerEventData)
    {

    }

    void IPointerExitHandler.OnPointerExit(PointerEventData pointerEventData)
    {

    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (villageBuildSelection != null)
        {
            villageBuildSelection.highlighted = false;
            villageBuildSelection.selectedVillageBuildingInfo.gameObject.SetActive(false);
        }
        
        defaultTexts.SetActive(true);
        buildingSelected = false;
        villageBuildSelection = null;
    }
}
