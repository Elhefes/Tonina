using UnityEngine;
using UnityEngine.EventSystems;

public class VillageBuildSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public SelectedVillageBuildingInfo selectedVillageBuildingInfo;
    public VillageBuildMenu villageBuildMenu;
    public GameObject defaultTexts;
    public GameObject optionalSelection;

    public bool isHovered;
    public bool highlighted;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData pointerEventData)
    {
        if (!villageBuildMenu.buildingSelected)
        {
            highlighted = true;
            isHovered = true;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData pointerEventData)
    {
        if (!villageBuildMenu.buildingSelected)
        {
            highlighted = false;
            defaultTexts.SetActive(true);
        }
        isHovered = false;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (isHovered)
        {
            if (highlighted)
            {
                villageBuildMenu.buildingSelected = true;
                villageBuildMenu.villageBuildSelection = this;
            }
        }
    }

    void Update()
    {
        if (highlighted)
        {
            selectedVillageBuildingInfo.gameObject.SetActive(true);
            defaultTexts.SetActive(false);
        }
        else
        {
            selectedVillageBuildingInfo.gameObject.SetActive(false);
        }
    }
}
