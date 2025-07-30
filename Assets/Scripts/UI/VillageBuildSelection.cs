using UnityEngine;
using UnityEngine.EventSystems;

public class VillageBuildSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public SelectedVillageBuildingInfo selectedVillageBuildingInfo;
    public VillageBuildMenu villageBuildMenu;
    public GameObject defaultTexts;
    public GameObject optionalSelection;

    public bool highlighted;
    private bool isHovered;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData pointerEventData)
    {
        if (!villageBuildMenu.buildingSelected)
        {
            highlighted = true;
        }
        isHovered = true;
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
            }
            else
            {
                villageBuildMenu.UnselectAll();
                highlighted = true;
                villageBuildMenu.buildingSelected = true;
            }
            villageBuildMenu.buyButtonObject.SetActive(true);
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
