using UnityEngine;
using UnityEngine.EventSystems;

public class VillageBuildSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public SelectedVillageBuildingInfo selectedVillageBuildingInfo;
    public VillageBuildMenu villageBuildMenu;

    public bool highlighted;
    private bool isHovered;

    public GameObject[] boughtObjectsToEnable;
    public GameObject[] boughtObjectsToDisable;

    public bool buildsNextFloor;
    public PyramidObjectsProgression pyramidObjectsProgression;

    public VillageTeleportMenu villageTPMenu;

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
            villageBuildMenu.defaultTexts.SetActive(true);
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
            villageBuildMenu.currentBuildSelection = this;
            villageBuildMenu.buyButtonObject.SetActive(true);
        }
    }

    void Update()
    {
        if (highlighted)
        {
            selectedVillageBuildingInfo.gameObject.SetActive(true);
            villageBuildMenu.defaultTexts.SetActive(false);
        }
        else
        {
            selectedVillageBuildingInfo.gameObject.SetActive(false);
        }
    }

    public void BuyThisSelection()
    {
        int extraFloorsBuilt = GameState.Instance.progressionData.extraPyramidFloorsBuilt;

        if (boughtObjectsToEnable != null)
        {
            if (boughtObjectsToEnable.Length > 0)
            {
                foreach (GameObject obj in boughtObjectsToEnable)
                {
                    obj.SetActive(true);
                }
            }
        }

        if (boughtObjectsToDisable != null)
        {
            if (boughtObjectsToDisable.Length > 0)
            {
                foreach (GameObject obj in boughtObjectsToDisable)
                {
                    obj.SetActive(false);
                }
            }
        }

        if (pyramidObjectsProgression != null)
        {
            if (buildsNextFloor)
            {
                pyramidObjectsProgression.BuildNextPyramidLevel();
                villageBuildMenu.toninaCutSceneCamera.MoveCameraToTemporaryPosition(false, extraFloorsBuilt, 
                    Camera.main.transform, Camera.main.transform.rotation, Camera.main.farClipPlane);
            }
            else
            {
                villageBuildMenu.toninaCutSceneCamera.MoveCameraToTemporaryPosition(true, extraFloorsBuilt, 
                    Camera.main.transform, Camera.main.transform.rotation, Camera.main.farClipPlane);
            }
            villageBuildMenu.gameObject.SetActive(false);
        }

        if (villageTPMenu != null)
        {
            if (buildsNextFloor)
            {
                villageTPMenu.UpdateExtraFloorsInt();
                villageTPMenu.UpdateMiddleGatePosition();
            }
        }
    }
}
