using System.Collections.Generic;
using UnityEngine;

public class BuildingRemover : MonoBehaviour
{
    public List<PlaceableBuilding> removableBuildings = new List<PlaceableBuilding>();

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
            foreach (PlaceableBuilding building in removableBuildings)
            {
                Destroy(building.gameObject);
            }
            removableBuildings.Clear();
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
