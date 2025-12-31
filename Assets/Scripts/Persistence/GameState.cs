using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public ProgressionData progressionData;
    public PlaceablesManager placeablesManager;

    private void Awake()
    {
        Instance = this;
        LoadWorld();
    }

    public void SaveWorld()
    {
        if (progressionData == null) progressionData = new ProgressionData();

        PlaceablesData placeablesData = placeablesManager.GeneratePlaceablesData();

        WorldData world = new WorldData(progressionData, placeablesData);
        WorldSaveLoad.SaveWorldData(world);
    }

    public void LoadWorld()
    {
        WorldData world = WorldSaveLoad.LoadWorldData();
        if (world == null)
        {
            Debug.Log("No world save found, creating new world...");
            progressionData = new ProgressionData();
            return;
        }

        progressionData = world.progression;
        placeablesManager.ApplyPlaceablesData(world.placeables);
    }

    public void DeleteWorld(int fileSlot)
    {
        StatsSaveLoad.DeleteStats(fileSlot);
        WorldSaveLoad.DeleteSave(fileSlot);
    }
}
