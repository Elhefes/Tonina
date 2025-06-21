using UnityEngine;

public class PyramidObjectsProgression : MonoBehaviour
{
    public GameObject kingHouse;
    public int extraFloorsBuilt;

    public Vector3[] kingHousePositionsFrom4thTo14th;

    [Header("Pyramid Levels")]
    public GameObject lvl4Floor;
    public GameObject lvl4Walls;

    [Header("Terrains")]
    public Terrain lvl3Terrain;
    public Terrain lvl4Terrain;

    [Header("NavMesh surfaces")]
    public GameObject lvl3NavMeshSurface;
    public GameObject lvl3WeatherTempleNavMeshSurface;
    public GameObject lvl4NavMeshSurface;

    [Header("Temporary objects in village")]
    public GameObject lvl3TemporaryVillageObject;
    public GameObject lvl4TemporaryVillageObject;

    [Header("Fences")]
    public GameObject lvl3BattlefieldFence;
    public GameObject lvl4BattlefieldFence;
    public GameObject lvl4VillageFence;

    [Header("Stairs")]
    public GameObject lvl4Stairs;

    [Header("Border bumps")]
    public GameObject lvl4BorderBumps;

    [Header("Camera map colliders")]
    public GameObject lvl3BfCollider;
    public GameObject lvl3VillageCollider;
    public GameObject lvl4BfCollider;
    public GameObject lvl4VillageCollider;

    [Header("Weather Temple objects")]
    public GameObject weatherTemple;
    public GameObject weatherTempleObjToDisable;
    public GameObject[] weatherTempleObjectsToEnable;

    private void Start()
    {
        extraFloorsBuilt = 0; // Change this when save/load works
        UpdateKingHousePosition();
    }

    public void BuildNextPyramidLevel()
    {
        // very placeholder code

        lvl4Floor.SetActive(true);
        lvl4Walls.SetActive(true);

        lvl3Terrain.gameObject.SetActive(false);
        lvl4Terrain.gameObject.SetActive(true);

        lvl3NavMeshSurface.SetActive(false);
        lvl3WeatherTempleNavMeshSurface.SetActive(false);
        lvl4NavMeshSurface.SetActive(true);

        lvl3TemporaryVillageObject.SetActive(false);

        lvl3BattlefieldFence.SetActive(false);
        lvl4BattlefieldFence.SetActive(true);
        lvl4VillageFence.SetActive(true);

        lvl4Stairs.SetActive(true);

        lvl4BorderBumps.SetActive(true);

        lvl3BfCollider.SetActive(false);
        lvl3VillageCollider.SetActive(false);
        lvl4BfCollider.SetActive(true);
        lvl4VillageCollider.SetActive(true);

        weatherTempleObjToDisable.SetActive(false);
        weatherTemple.SetActive(true);
        foreach (GameObject obj in weatherTempleObjectsToEnable) obj.SetActive(true);

        extraFloorsBuilt++;
        UpdateKingHousePosition();
    }

    void UpdateKingHousePosition()
    {
        if (extraFloorsBuilt > 0) kingHouse.transform.position = kingHousePositionsFrom4thTo14th[extraFloorsBuilt - 1];
    }
}
