using UnityEngine;
using UnityEngine.EventSystems;

public class VillageBuildMenu : MonoBehaviour, IPointerDownHandler
{
    public GameObject defaultTexts;
    public GameObject buyButtonObject;
    public bool buildingSelected;

    public PyramidObjectsProgression pyramidObjectsProgression;

    public VillageBuildSelection[] villageBuildSelections;
    public VillageBuildSelection currentBuildSelection;

    public GameObject[] boughtFloorSprites;
    public GameObject[] boughtSpecialBuildingSprites;

    public ToninaCutSceneCamera toninaCutSceneCamera;

    private void OnEnable()
    {
        UpdateAllSelections();
    }

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

    private void UpdateAllSelections()
    {
        int extraFloorsBuilt = GameState.Instance.progressionData.extraPyramidFloorsBuilt;
        int specialsBuildingsBuilt = GameState.Instance.progressionData.specialBuildingsBuilt;

        foreach (GameObject obj in boughtFloorSprites) obj.SetActive(false);
        foreach (GameObject obj in boughtSpecialBuildingSprites) obj.SetActive(false);

        if (extraFloorsBuilt > 0)
        {
            for (int i = 0; i < extraFloorsBuilt; ++i)
            {
                boughtFloorSprites[i].SetActive(true);
            }
        }
        if (specialsBuildingsBuilt > 0)
        {
            for (int i = 0; i < specialsBuildingsBuilt; ++i)
            {
                boughtSpecialBuildingSprites[i].SetActive(true);
            }
        }
    }
}
