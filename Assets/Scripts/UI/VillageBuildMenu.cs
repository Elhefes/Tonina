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

        if (extraFloorsBuilt > 0)
        {
            for (int i = 0; i < extraFloorsBuilt; ++i)
            {
                boughtFloorSprites[i].SetActive(true);
                boughtFloorSelections[i].SetActive(false);
            }
        }
        if (specialsBuildingsBuilt > 0)
        {
            for (int i = 0; i < specialsBuildingsBuilt; ++i)
            {
                boughtSpecialBuildingSprites[i].SetActive(true);
                boughtSelectionsWithSpecials[i * 2].SetActive(false); // Special selection
                boughtSelectionsWithSpecials[i * 2 + 1].SetActive(false); // Special + floor selection
            }
        }
    }
}
