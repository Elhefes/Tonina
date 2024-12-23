using System.Collections.Generic;
using UnityEngine;

public class BuildingRemover : MonoBehaviour
{
    public BuildingWheel buildingWheel;
    private List<PlaceableBuilding> removableBuildings = new List<PlaceableBuilding>();

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

    public void RemoveSelectedBuildings()
    {
        if (removableBuildings != null)
        {
            int i = 0;
            foreach (PlaceableBuilding building in removableBuildings)
            {
                Destroy(building.gameObject);
                i++;
            }
            removableBuildings.Clear();
            buildingWheel.RemoveBuildingsByAmount(i);
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
