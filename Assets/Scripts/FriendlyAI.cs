using System.Collections;
using UnityEngine;

public class FriendlyAI : MonoBehaviour
{
    public CreatureMovement creatureMovement;
    private Coroutine AI_ControllerCoroutine;
    private Player player;
    private float normalStoppingDistance;
    //public Vector3 guardingSpot;
    //private bool guarding;

    void Awake()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void OnEnable()
    {
        if (player == null) player = FindFirstObjectByType<Player>(); // In attack mode
        normalStoppingDistance = creatureMovement.agent.stoppingDistance;
        AI_ControllerCoroutine = StartCoroutine(PeriodicalTargetChecking());
    }

    private IEnumerator PeriodicalTargetChecking()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            while (true)
            {
                if (NearestTarget("Enemy", 30f) != null)
                {
                    creatureMovement.agent.stoppingDistance = normalStoppingDistance;
                    break;
                }
                //else if (Vector3.Distance(transform.position, player.transform.position) < 33f)
                //{
                //    FollowPlayerDirections();
                //    break;
                //}
                FollowPlayerDirections();

                // If player is too far away, it should guard the position realistically
                // Need to figure out how to prevent clumping

                //if (!guarding)
                //{
                //    guardingSpot = transform.position;
                //}
                //GuardThisPlace();
                //yield return new WaitForSeconds(2f);
                break;
            }
        }
    }

    public GameObject NearestTarget(string tag, float range)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject obj in objectsWithTag)
        {
            if (!obj.activeInHierarchy) continue; // Ignore disabled objects

            float distance = Vector3.Distance(transform.position, obj.transform.position);

            if (distance < nearestDistance && distance <= range)
            {
                nearestObject = obj;
                nearestDistance = distance;
            }
        }

        if (nearestObject != null)
        {
            creatureMovement.target = nearestObject.transform;
            //guarding = false;
        }
        return nearestObject;
    }

    void FollowPlayerDirections()
    {
        creatureMovement.target = null;
        if (player == null) return;

        if (player.destination == Vector3.zero) creatureMovement.agent.destination = player.transform.position;
        else creatureMovement.agent.destination = player.destination;
        creatureMovement.agent.stoppingDistance = 2.8f;
        //guarding = false;
    }

    //void GuardThisPlace()
    //{
    //    if (Mathf.Abs(creatureMovement.agent.velocity.x) > 0.25f && Mathf.Abs(creatureMovement.agent.velocity.y) > 0.25f && Mathf.Abs(creatureMovement.agent.velocity.z) > 0.25f) return;
    //    guarding = true;
    //    creatureMovement.agent.destination = new Vector3(guardingSpot.x + Random.Range(0.8f, 2.4f), 0f, guardingSpot.z + Random.Range(0.8f, 1.8f));
    //}
}
