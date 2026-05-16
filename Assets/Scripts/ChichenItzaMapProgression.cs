using System.Collections;
using UnityEngine;

public class ChichenItzaMapProgression : MonoBehaviour
{
    public Player player;
    public GameplayCameraAngles camAngles;

    // Pickables
    public GameObject pyramidKey;
    public GameObject pickableCamazoIcon;
    public GameObject pickableMask;
    public GameObject pickableKukulkans;

    // Moving elements
    public GameObject camazoSouthWall;
    public GameObject camazoEastWall;
    public GameObject kukulkansInElCastillo;

    // Enemy clusters
    public GameObject firstEnemyCluster;

    private bool firstEnemyAttackStarted;
    private bool firstSectionComplete;
    private bool secondSectionComplete;

    private void Start()
    {
        player.EnableBattleMode();
    }

    private void Update()
    {
        if (!pyramidKey.activeInHierarchy)
        {
            if (camazoSouthWall.activeInHierarchy)
            {
                if (Vector3.Distance(player.transform.position, camazoSouthWall.transform.position) < 2f) camazoSouthWall.SetActive(false);
            }
            else if (camazoEastWall.activeInHierarchy)
            {
                if (Vector3.Distance(player.transform.position, camazoEastWall.transform.position) < 2f) camazoEastWall.SetActive(false);
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
            }
        }
    }

    private IEnumerator FirstEnemyAttack()
    {
        yield return new WaitForSeconds(5.5f);
        firstEnemyCluster.SetActive(true);
    }
}