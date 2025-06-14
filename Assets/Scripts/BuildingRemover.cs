using System.Collections.Generic;
using UnityEngine;

public class BuildingRemover : MonoBehaviour
{
    public BuildingWheel buildingWheel;
    private List<PlaceableBuilding> removableBuildings = new List<PlaceableBuilding>();
    private List<GameObject> hiddenBuildings = new List<GameObject>();

    private void OnEnable()
    {
        removableBuildings.Clear();
    }

    public void SelectBuilding(PlaceableBuilding building)
    {
        if (building != null)
        {
            if (!building.removingBuildingCoat.activeSelf)
            {
                building.removingBuildingCoat.SetActive(true);
                removableBuildings.Add(building);
            }
            else
            {
                building.removingBuildingCoat.SetActive(false);
                removableBuildings.Remove(building);
            }
        }
    }

    public void HideSelectedBuildings()
    {
        if (removableBuildings != null)
        {
            int i = 0;
            foreach (PlaceableBuilding building in removableBuildings)
            {
                if (building.gameObject.activeSelf)
                {
                    hiddenBuildings.Add(building.gameObject);
                    building.gameObject.SetActive(false);
                    buildingWheel.incomingCost -= building.cost;
                    buildingWheel.UpdateIncomingCostText();
                    buildingWheel.placedObjectsGrid.placedObjectAmounts[building.buildingIndex] -= 1;
                    buildingWheel.placedObjectsGrid.UpdatePlacedBuildingIndicator(building.buildingIndex);
                    i++;
                }
            }
            buildingWheel.RemoveBuildingsByAmount(i);
        }
    }

    public void DestroyHiddenBuildings()
    {
        if (hiddenBuildings != null)
        {
            foreach (GameObject building in hiddenBuildings)
            {
                Destroy(building);
                buildingWheel.statsController.changesToBattlefield++;
            }
            removableBuildings.Clear();
            hiddenBuildings.Clear();
        }
    }

    public void RestoreHiddenBuildings()
    {
        if (hiddenBuildings != null)
        {
            foreach (GameObject building in hiddenBuildings)
            {
                building.SetActive(true);
            }
            removableBuildings.Clear();
            hiddenBuildings.Clear();
        }
    }

    public void ExitFromRemoving()
    {
        if (removableBuildings != null)
        {
            foreach (PlaceableBuilding building in removableBuildings)
            {
                if (building.removingBuildingCoat.activeSelf)
                {
                    building.removingBuildingCoat.SetActive(false);
                }
            }
            removableBuildings.Clear();
        }
    }
}
