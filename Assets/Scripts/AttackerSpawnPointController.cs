using UnityEngine;

public class AttackerSpawnPointController : MonoBehaviour
{
    public float battleAreaWidth;

    public Transform leftestSpawnPointTransform;
    public Transform rightestSpawnPointTransform;
    public Transform[] spawnPointTransformsInBetween;

    public Vector3[] leftestSpawnPoints;
    public Vector3[] rightestSpawnPoints;
    public Vector3 centerSpawnPoint; // = Spawn4

    public GameObject clubber;

    private void Start()
    {
        // Change this when save/load is implemented
        UpdateSpawnPositions(0);

        //TestSpawns(); // Uncomment this to test if enemies (agents) spawn correctly on their positions
    }

    public void UpdateBattleAreaWidth(float newWidth) { battleAreaWidth = newWidth; }

    public void UpdateSpawnPositions(int extraFloorsBuilt)
    {
        leftestSpawnPointTransform.position = leftestSpawnPoints[extraFloorsBuilt];
        rightestSpawnPointTransform.transform.position = rightestSpawnPoints[extraFloorsBuilt];

        float equalDivision = 2 * battleAreaWidth / spawnPointTransformsInBetween.Length;

        for (int i = 0; i < spawnPointTransformsInBetween.Length; i++)
        {
            if (i < spawnPointTransformsInBetween.Length / 2)
            {
                spawnPointTransformsInBetween[i].position = new Vector3(-battleAreaWidth + i * equalDivision, 
                    centerSpawnPoint.y, centerSpawnPoint.z);
            }
            else
            {
                spawnPointTransformsInBetween[i].position = new Vector3(centerSpawnPoint.x + equalDivision * 
                    (i + 1 - spawnPointTransformsInBetween.Length / 2), centerSpawnPoint.y, centerSpawnPoint.z);
            }
        }
    }

    private void TestSpawns()
    {
        Instantiate(clubber, leftestSpawnPointTransform.position, leftestSpawnPointTransform.rotation);
        Instantiate(clubber, centerSpawnPoint, leftestSpawnPointTransform.rotation);
        Instantiate(clubber, rightestSpawnPointTransform.position, rightestSpawnPointTransform.rotation);

        foreach (Transform t in spawnPointTransformsInBetween)
        {
            Instantiate(clubber, t.position, t.rotation);
        }
    }
}
