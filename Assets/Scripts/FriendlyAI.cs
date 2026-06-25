using System.Collections;
using UnityEngine;

public class FriendlyAI : MonoBehaviour
{
    public Creature friendlyCreature;
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
        normalStoppingDistance = friendlyCreature.creatureMovement.agent.stoppingDistance;
        StartCoroutine(PeriodicalTargetChecking());
    }

    private IEnumerator PeriodicalTargetChecking()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            while (true)
            {
                friendlyCreature.creatureMovement.agent.stoppingDistance = normalStoppingDistance;

                if (NearestTarget("Enemy", 8f) != null) break;
                else if (friendlyCreature.isAttacker && NearestTarget("Barricade", 10f) != null) break;
                else if (NearestTarget("Enemy", 30f) != null) break;

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
            friendlyCreature.creatureMovement.target = nearestObject.transform;
            //guarding = false;
        }
        return nearestObject;
    }

    void FollowPlayerDirections()
    {
        friendlyCreature.creatureMovement.target = null;
        if (player == null) return;

        if (player.destination == Vector3.zero) friendlyCreature.creatureMovement.MoveToDestination(player.transform.position);
        else friendlyCreature.creatureMovement.MoveToDestination(player.destination);
        friendlyCreature.creatureMovement.agent.stoppingDistance = 2.8f;
        //guarding = false;
    }

    //void GuardThisPlace()
    //{
    //    if (Mathf.Abs(creatureMovement.agent.velocity.x) > 0.25f && Mathf.Abs(creatureMovement.agent.velocity.y) > 0.25f && Mathf.Abs(creatureMovement.agent.velocity.z) > 0.25f) return;
    //    guarding = true;
    //    creatureMovement.agent.destination = new Vector3(guardingSpot.x + Random.Range(0.8f, 2.4f), 0f, guardingSpot.z + Random.Range(0.8f, 1.8f));
    //}
}
