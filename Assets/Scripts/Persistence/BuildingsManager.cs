using System.Collections.Generic;
using UnityEngine;

public class BuildingsManager : MonoBehaviour
{
    // This is incomplete: it should handle all saved placeable buildings. It should load the buildings at Start()
    // and it should contain public functions for resetting and destroying placed buildings.
    // Finding GameObjects / Prefabs - this can be changed or removed if there's a better way.

    private List<GameObject> placedBuildings = new List<GameObject>();
    public int buildingsPlaced;
    public int maxBuildingAmount;

    BuildingsData buildingsData;
    public GameObject[] buildingPrefabs; // Array of all building prefabs

    private void Start()
    {
        LoadBuildingsData();
        if (buildingsData != null) maxBuildingAmount = buildingsData.maxPlaceableBuildings;
        else maxBuildingAmount = 3;
    }

    public void LoadBuildingsData()
    {
        
    }

    public void LoadPlacedBuildings()
    {
        foreach (var buildingData in buildingsData.placeableBuildings)
        {
            GameObject prefab = FindPrefabByName(buildingData.obj);
            if (prefab != null)
            {
                Instantiate(prefab, buildingData.position, buildingData.rotation);
            }
            else
            {
                Debug.LogWarning($"Prefab not found: {buildingData.obj}");
            }
        }
    }

    private GameObject FindPrefabByName(GameObject obj)
    {
        foreach (var prefab in buildingPrefabs)
        {
            if (prefab == obj)
                return prefab;
        }
        return null;
    }

    private List<GameObject> FindPlacedBuildings()
    {
        GameObject[] objectsWithName = GameObject.FindGameObjectsWithTag("PlaceableBuilding");

        placedBuildings.Clear();

        foreach (GameObject obj in objectsWithName)
        {
            if (obj.transform.parent == null) placedBuildings.Add(obj);

        }
        return placedBuildings;
    }

    public void UpdateExistingBuildingsAmount()
    {
        if (FindPlacedBuildings() == null) buildingsPlaced = 0;
        else buildingsPlaced = FindPlacedBuildings().Count;
    }

    public void ResetPlacedBuildings()
    {
        placedBuildings = FindPlacedBuildings();
        if (placedBuildings != null)
        {
            foreach (GameObject obj in placedBuildings)
            {
                obj.SetActive(false);
                obj.SetActive(true);
            }
        }
    }

    public void DestroyPlacedBuildings()
    {
        placedBuildings = FindPlacedBuildings();
        if (placedBuildings != null)
        {
            foreach (GameObject obj in placedBuildings)
            {
                Destroy(obj);
            }
        }
    }
}
