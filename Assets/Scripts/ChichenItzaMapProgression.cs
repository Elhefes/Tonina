using System.Collections;
using UnityEngine;

public class ChichenItzaMapProgression : MonoBehaviour
{
    public Player player;
    public GameplayCameraAngles camAngles;
    public FriendlyCluster friendlyCluster;
    public GameObject battleUI;
    public GameObject objectPooler;

    // Pickables
    public GameObject pyramidKey;
    public GameObject pickableCamazoIcon;
    public GameObject pickableMask;
    public GameObject pickableKukulkans;

    // Moving elements
    public GameObject camazoSouthWall;
    public GameObject camazoEastWall;
    public GameObject kukulkansInElCastillo;
    public GameObject elCastilloBlocker;

    // Enemy clusters
    public EnemyCluster firstEnemyCluster;
    public EnemyCluster secondEnemyCluster;

    // Infinite enemy spawn points
    public Transform[] forestSpawns;
    public Transform[] elCastilloSpawns;

    private bool firstEnemyAttackStarted;
    private bool secondEnemyAttackStarted;
    private bool firstSectionComplete;
    private bool secondSectionComplete;

    private Vector3 playerSpawnPosition;

    private void Start()
    {
        player.EnableBattleMode();
        playerSpawnPosition = player.transform.position;
    }

    public void ResetChichenItza()
    {
        StopAllCoroutines();
        camAngles.SetCameraAngle(0);
        pyramidKey.SetActive(true);
        pickableMask.SetActive(true);
        pickableKukulkans.SetActive(true);
        camazoSouthWall.SetActive(true);
        camazoEastWall.SetActive(true);
        kukulkansInElCastillo.SetActive(false);
        elCastilloBlocker.SetActive(true);
        firstEnemyAttackStarted = false;
        secondEnemyAttackStarted = false;
        firstSectionComplete = false;
        secondSectionComplete = false;
        DisableEverythingInObjectPooler();
        player.transform.position = playerSpawnPosition;
        player.transform.rotation = Quaternion.Euler(camAngles.presetCameraAngles[0].rotation.eulerAngles);
        friendlyCluster.ResetAllFriendlies();
    }

    private void DisableEverythingInObjectPooler()
    {
        foreach (Transform child in objectPooler.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!pyramidKey.activeInHierarchy)
        {
            if (camazoSouthWall.activeInHierarchy)
            {
                if (Vector3.Distance(player.transform.position, camazoSouthWall.transform.position) < 2.8f) camazoSouthWall.SetActive(false);
            }
            if (camazoEastWall.activeInHierarchy)
            {
                if (Vector3.Distance(player.transform.position, camazoEastWall.transform.position) < 2.8f) camazoEastWall.SetActive(false);
            }
        }

        if (!secondEnemyAttackStarted)
        {
            if (player.transform.position.x < -72f)
            {
                StartCoroutine(SecondEnemyAttack());
                secondEnemyAttackStarted = true;
            }
        }

        if (!firstEnemyAttackStarted)
        {
            if (player.transform.position.z > -50f)
            {
                StartCoroutine(FirstEnemyAttack());
                firstEnemyAttackStarted = true;
            }
        }

        // Placeholder for testing gameplay loop
        else if (!firstSectionComplete)
        {
            if (player.transform.position.x < 123f && player.transform.position.z > 0f)
            {
                camAngles.SetCameraAngle(1);
                StartCoroutine(SpawnEnemiesInfinitely());
                firstSectionComplete = true;
            }
        }
        else if (!secondSectionComplete)
        {
            // pyramid key is placeholder check -> change to camazo icon
            if (!pyramidKey.activeInHierarchy && !pickableMask.activeInHierarchy && !pickableKukulkans.activeInHierarchy)
            {
                camAngles.SetCameraAngle(2);
                kukulkansInElCastillo.SetActive(true);
                secondSectionComplete = true;
                elCastilloBlocker.SetActive(false);
            }
        }
    }

    private IEnumerator FirstEnemyAttack()
    {
        yield return new WaitForSeconds(4.5f);
        firstEnemyCluster.SpawnClusterFromPool();
    }

    private IEnumerator SecondEnemyAttack()
    {
        yield return new WaitForSeconds(2f);
        secondEnemyCluster.SpawnClusterFromPool();
    }

    private IEnumerator SpawnEnemiesInfinitely()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));

            Vector3 spawnPoint;

            // 75% ground, 25% forest
            if (Random.value < 0.75f)
            {
                spawnPoint = elCastilloSpawns[Random.Range(0, elCastilloSpawns.Length)].position;
            }
            else
            {
                spawnPoint = forestSpawns[Random.Range(0, forestSpawns.Length)].position;
            }

            // Spawn random enemy

            char c = (char)Random.Range('A', 'E');

            string enemyType = "Clubber";
            if (c.Equals('B')) enemyType = "Runner";
            if (c.Equals('C')) enemyType = "SpearWarrior";
            if (c.Equals('D')) enemyType = "AxeWarrior";

            Enemy enemy = ObjectPooler.Instance.SpawnEnemyFromPool(enemyType, spawnPoint, Quaternion.Euler(Vector3.zero));
            enemy.ResetEnemyAttributes();
        }
    }
}