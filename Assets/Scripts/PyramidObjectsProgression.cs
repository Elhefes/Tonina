using UnityEngine;

public class PyramidObjectsProgression : MonoBehaviour
{
    public GameObject kingHouse;
    public int extraFloorsBuilt;
    public int specialBuildingsBuilt; // Weather Temple, Maize Farmers House... etc.

    public PlaceablesManager placeablesManager;
    public AttackerSpawnPointController attackerSpawnPointController;
    public VillageTeleportMenu villageTeleportMenu;

    public Vector3[] kingHousePositionsFrom4thTo14th;

    [Header("Pyramid Levels")]
    public GameObject[] pyramidExtraFloors;
    public GameObject[] pyramidExtraFloorWalls;

    [Header("Terrains")]
    public Terrain[] terrains;

    [Header("NavMesh surfaces")]
    public GameObject[] navMeshSurfaces;

    [Header("Temporary objects in village")]
    public GameObject lvl3TemporaryVillageObject;

    [Header("Villagers")]
    public GameObject[] villagers;

    [Header("Village houses")]
    public GameObject[] villageHouses;

    [Header("Special buildings")]
    public GameObject[] specialBuildings;

    [Header("Fences")]
    public GameObject[] villageFences;
    public GameObject[] battleFieldFences;

    [Header("Stairs")]
    public GameObject[] stairs;

    [Header("Border bumps")]
    public GameObject[] borderBumps;

    [Header("Camera map colliders")]
    public GameObject[] villageCamColliders;
    public GameObject[] battleFieldCamColliders;

    [Header("Weather Temple objects")]
    public GameObject weatherTemple;
    public GameObject weatherTempleLvl3Obj;
    public GameObject weatherTempleLvl4Obj;

    private void Start()
    {
        extraFloorsBuilt = GameState.Instance.progressionData.extraPyramidFloorsBuilt;
        specialBuildingsBuilt = GameState.Instance.progressionData.specialBuildingsBuilt;
        EnableAllCurrentPyramidObjects();
    }

    public void BuildNextPyramidLevel()
    {
        extraFloorsBuilt++;
        EnableAllCurrentPyramidObjects();
        //buildingsManager.maxBuildingAmount += 2;
        GameState.Instance.progressionData.extraPyramidFloorsBuilt = extraFloorsBuilt;
        GameState.Instance.SaveWorld();
    }

    public void BuildNextSpecialBuilding()
    {
        specialBuildingsBuilt++;
        specialBuildings[specialBuildingsBuilt - 1].SetActive(true);
        EnableCurrentNavMesh();
        GameState.Instance.progressionData.specialBuildingsBuilt = specialBuildingsBuilt;
        GameState.Instance.SaveWorld();
    }

    private void EnableAllCurrentPyramidObjects()
    {
        // Manual check for how many special buildings there should be
        if (extraFloorsBuilt > 1) specialBuildingsBuilt = 2;
        else if (extraFloorsBuilt > 0) specialBuildingsBuilt = 1;

        EnablePyramidLevels();
        EnableCurrentTerrain();
        EnableCurrentNavMesh();
        EnableVillagers();
        EnableVillageHouses();
        EnableSpecialBuildings();
        EnableCurrentFences();
        EnableStairs();
        EnableBorderBumps();
        EnableCurrentCamColliders();
        UpdateKingHousePosition();

        villageTeleportMenu.UpdateExtraFloorsInt();
        villageTeleportMenu.UpdateMiddleGatePosition();

        if (extraFloorsBuilt > 0)
        {
            lvl3TemporaryVillageObject.SetActive(false);
            // Change Weather Temple accessibility points when there's 4 or more floors
            weatherTempleLvl3Obj.SetActive(false);
            weatherTempleLvl4Obj.SetActive(true);
        }
    }

    private void EnablePyramidLevels()
    {
        if (extraFloorsBuilt > 0)
        {
            for (int i = 0; i < extraFloorsBuilt; ++i)
            {
                pyramidExtraFloors[i].SetActive(true);
                pyramidExtraFloorWalls[i].SetActive(true);
            }
        }
    }

    private void EnableCurrentTerrain()
    {
        foreach (Terrain terrain in terrains) terrain.gameObject.SetActive(false);
        terrains[extraFloorsBuilt].gameObject.SetActive(true);
    }

    private void EnableCurrentNavMesh()
    {
        foreach (GameObject surface in navMeshSurfaces) surface.SetActive(false);
        navMeshSurfaces[extraFloorsBuilt + specialBuildingsBuilt].SetActive(true);
    }

    private void EnableVillagers()
    {
        if (extraFloorsBuilt > 0)
        {
            for (int i = 0; i < extraFloorsBuilt; ++i)
            {
                villagers[i].SetActive(true);
            }
        }
    }

    private void EnableVillageHouses()
    {
        if (extraFloorsBuilt > 0)
        {
            for (int i = 0; i < extraFloorsBuilt; ++i)
            {
                villageHouses[i].SetActive(true);
            }
        }
    }

    private void EnableSpecialBuildings()
    {
        if (specialBuildingsBuilt > 0)
        {
            for (int i = 0; i < specialBuildingsBuilt; ++i)
            {
                specialBuildings[i].SetActive(true);
            }
        }
    }

    private void EnableCurrentFences()
    {
        foreach (GameObject fence in villageFences) fence.SetActive(false);
        if (extraFloorsBuilt > 0) villageFences[extraFloorsBuilt - 1].gameObject.SetActive(true); // Lvl3 village fences are permanent

        foreach (GameObject fence in battleFieldFences) fence.SetActive(false);
        battleFieldFences[extraFloorsBuilt].gameObject.SetActive(true);
    }

    private void EnableStairs()
    {
        if (extraFloorsBuilt > 0)
        {
            for (int i = 0; i < extraFloorsBuilt; ++i)
            {
                stairs[i].SetActive(true);
            }
        }
    }

    private void EnableBorderBumps()
    {
        if (extraFloorsBuilt > 0)
        {
            for (int i = 0; i < extraFloorsBuilt; ++i)
            {
                borderBumps[i].SetActive(true);
            }
        }
    }

    private void EnableCurrentCamColliders()
    {
        foreach (GameObject camCollider in villageCamColliders) camCollider.SetActive(false);
        villageCamColliders[extraFloorsBuilt].gameObject.SetActive(true);

        foreach (GameObject camCollider in battleFieldCamColliders) camCollider.SetActive(false);
        battleFieldCamColliders[extraFloorsBuilt].gameObject.SetActive(true);
    }

    private void UpdateKingHousePosition()
    {
        if (extraFloorsBuilt > 0)
        {
            kingHouse.SetActive(false);
            kingHouse.transform.position = kingHousePositionsFrom4thTo14th[extraFloorsBuilt - 1];
            kingHouse.SetActive(true);
        }
    }
}
