using UnityEngine;

public class AttackerSpawnPointController : MonoBehaviour
{
    public GameObject[] spawnPointParents;
    public Transform[] spawnPoints;
    public GameObject clubber;

    private void Start()
    {
        UpdateSpawnPositions(GameState.Instance.progressionData.extraPyramidFloorsBuilt);

        //TestSpawns(); // Uncomment this to test if enemies (agents) spawn correctly on their positions
    }

    public void UpdateSpawnPositions(int extraFloorsBuilt)
    {
        foreach (GameObject obj in spawnPointParents) obj.SetActive(false);
        spawnPointParents[extraFloorsBuilt].SetActive(true);

        // Set spawn point transforms to the ones that are of the active parent
        Transform activeParent = GetActiveParent();
        if (activeParent == null) return;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].position = activeParent.GetChild(i).position;
            spawnPoints[i].rotation = activeParent.GetChild(i).rotation;
        }
    }

    private Transform GetActiveParent()
    {
        foreach (GameObject p in spawnPointParents)
        {
            if (p.activeInHierarchy)
                return p.transform;
        }
        return null;
    }

    private void TestSpawns()
    {
        for (int i = 0; i < spawnPoints.Length; ++i)
        {
            Instantiate(clubber, spawnPoints[i].position, spawnPoints[i].rotation);
        }
    }
}
