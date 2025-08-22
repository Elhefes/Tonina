using UnityEngine;

public class PyramidObjectsProgression : MonoBehaviour
{
    public GameObject kingHouse;
    public int extraFloorsBuilt;

    public BuildingsManager buildingsManager;
    public EnemySpawnPointController enemySpawnPointController;

    public Vector3[] kingHousePositionsFrom4thTo14th;

    [Header("Pyramid Levels")]
    public GameObject lvl4Floor;
    public GameObject lvl4Walls;

    public GameObject lvl5Floor;
    public GameObject lvl5Walls;

    [Header("Terrains")]
    public Terrain lvl3Terrain;
    public Terrain lvl4Terrain;
    public Terrain lvl5Terrain;

    [Header("NavMesh surfaces")]
    public GameObject lvl3NavMeshSurface;
    public GameObject lvl3WeatherTempleNavMeshSurface;
    public GameObject lvl4NavMeshSurface;
    public GameObject lvl4MaizeFarmersHouseNavMeshSurface;
    public GameObject lvl5NavMeshSurface;

    [Header("Temporary objects in village")]
    public GameObject lvl3TemporaryVillageObject;

    [Header("Villagers")]
    public GameObject lvl4Villagers;
    public GameObject lvl5Villagers;

    [Header("Village houses")]
    public GameObject lvl4Houses;
    public GameObject lvl5Houses;

    [Header("Fences")]
    public GameObject lvl3BattlefieldFence;
    public GameObject lvl4BattlefieldFence;
    public GameObject lvl4VillageFence;
    public GameObject lvl5BattlefieldFence;
    public GameObject lvl5VillageFence;

    [Header("Stairs")]
    public GameObject lvl4Stairs;
    public GameObject lvl5Stairs;

    [Header("Border bumps")]
    public GameObject lvl4BorderBumps;
    public GameObject lvl5BorderBumps;

    [Header("Camera map colliders")]
    public GameObject lvl3BfCollider;
    public GameObject lvl3VillageCollider;
    public GameObject lvl4BfCollider;
    public GameObject lvl4VillageCollider;
    public GameObject lvl5BfCollider;
    public GameObject lvl5VillageCollider;

    [Header("Weather Temple objects")]
    public GameObject weatherTemple;
    public GameObject weatherTempleLvl3Obj;
    public GameObject weatherTempleLvl4Obj;

    private void Start()
    {
        extraFloorsBuilt = 0; // Change this when save/load works
        UpdateKingHousePosition();
    }

    public void BuildNextPyramidLevel()
    {
        // very placeholder code

        if (extraFloorsBuilt == 0)
        {
            lvl4Floor.SetActive(true);
            lvl4Walls.SetActive(true);

            lvl3Terrain.gameObject.SetActive(false);
            lvl4Terrain.gameObject.SetActive(true);

            lvl3NavMeshSurface.SetActive(false);
            lvl3WeatherTempleNavMeshSurface.SetActive(false);
            lvl4NavMeshSurface.SetActive(true);

            lvl3TemporaryVillageObject.SetActive(false);

            lvl4Villagers.SetActive(true);

            lvl4Houses.SetActive(true);

            lvl3BattlefieldFence.SetActive(false);
            lvl4BattlefieldFence.SetActive(true);
            lvl4VillageFence.SetActive(true);

            lvl4Stairs.SetActive(true);

            lvl4BorderBumps.SetActive(true);

            lvl3BfCollider.SetActive(false);
            lvl3VillageCollider.SetActive(false);
            lvl4BfCollider.SetActive(true);
            lvl4VillageCollider.SetActive(true);

            weatherTempleLvl3Obj.SetActive(false);
            weatherTemple.SetActive(true);
            weatherTempleLvl4Obj.SetActive(true);

            extraFloorsBuilt++;

            enemySpawnPointController.UpdateSpawnPositions(extraFloorsBuilt, 45f);
        }

        else if (extraFloorsBuilt == 1)
        {
            lvl5Floor.SetActive(true);
            lvl5Walls.SetActive(true);

            lvl4Terrain.gameObject.SetActive(false);
            lvl5Terrain.gameObject.SetActive(true);

            lvl4NavMeshSurface.SetActive(false);
            lvl4MaizeFarmersHouseNavMeshSurface.SetActive(false);
            lvl5NavMeshSurface.SetActive(true);

            lvl5Villagers.SetActive(true);

            lvl5Houses.SetActive(true);

            lvl4BattlefieldFence.SetActive(false);
            lvl5BattlefieldFence.SetActive(true);
            lvl4VillageFence.SetActive(false);
            lvl5VillageFence.SetActive(true);

            lvl5Stairs.SetActive(true);

            lvl5BorderBumps.SetActive(true);

            lvl4BfCollider.SetActive(false);
            lvl4VillageCollider.SetActive(false);
            lvl5BfCollider.SetActive(true);
            lvl5VillageCollider.SetActive(true);

            extraFloorsBuilt++;

            enemySpawnPointController.UpdateSpawnPositions(extraFloorsBuilt, 65f);
        }

        buildingsManager.maxBuildingAmount += 2;
        UpdateKingHousePosition();
    }

    void UpdateKingHousePosition()
    {
        if (extraFloorsBuilt > 0)
        {
            kingHouse.SetActive(false);
            kingHouse.transform.position = kingHousePositionsFrom4thTo14th[extraFloorsBuilt - 1];
            kingHouse.SetActive(true);
        }
    }
}
