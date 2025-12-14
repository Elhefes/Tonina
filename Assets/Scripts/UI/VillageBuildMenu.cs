using UnityEngine;
using UnityEngine.EventSystems;

public class VillageBuildMenu : MonoBehaviour, IPointerDownHandler
{
    public GameObject defaultTexts;
    public GameObject buyButtonObject;
    public bool buildingSelected;

    public PyramidObjectsProgression pyramidObjectsProgression;

    public VillageBuildSelection[] villageBuildSelections; // All possible selections
    public VillageBuildSelection currentBuildSelection;

    // Bought selections, i.e selectables -> non-interactable sprites
    public GameObject[] boughtFloorSelections; // Only floor selections
    public GameObject[] boughtSelectionsWithSpecials; // Special buildings + floors with special buildings
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
        foreach (GameObject obj in boughtFloorSelections) obj.SetActive(false);
        foreach (GameObject obj in boughtSelectionsWithSpecials) obj.SetActive(false);

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

        // Switch case is used because not all floors will have a special building
        switch (extraFloorsBuilt)
        {
            case 0:
                {
                    if (specialsBuildingsBuilt == 1) villageBuildSelections[2].gameObject.SetActive(true);
                    else
                    {
                        villageBuildSelections[1].gameObject.SetActive(true);
                        villageBuildSelections[0].gameObject.SetActive(true);
                    }
                    break;
                }
            case 1:
                {
                    if (specialsBuildingsBuilt == 2) villageBuildSelections[5].gameObject.SetActive(true);
                    else
                    {
                        villageBuildSelections[4].gameObject.SetActive(true);
                        villageBuildSelections[3].gameObject.SetActive(true);
                    }
                    break;
                }
        }
    }
}
