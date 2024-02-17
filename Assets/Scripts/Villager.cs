using System.Collections;
using UnityEngine;

public class Villager : CreatureMovement
{
    private Vector3 startingPosition;
    private Coroutine freeMovementCoroutine;
    public float freeSpaceX;
    public float freeSpaceZ;
    public float minWaitTime;
    public float maxWaitTime;

    public bool talking;
    public string[] textLines;

    private GameObject player;

    void Awake()
    {
        startingPosition = transform.position;
        freeMovementCoroutine = StartCoroutine(MoveRandomly());
    }

    private void Update()
    {
        base.Update();
        if (talking && player != null)
        {
            // Calculate the direction to the destination
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // Ignore the y-axis to prevent tilting
            direction.y = 0f;

            // Rotate towards the destination
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * agent.angularSpeed * 0.25f);
            }
        }
    }

    public void TalkToPlayer(GameObject p)
    {
        player = p;
        agent.destination = transform.position;
    }

    private IEnumerator MoveRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            if (!talking) agent.destination = new Vector3(startingPosition.x + Random.Range(-freeSpaceX, freeSpaceX), startingPosition.y, startingPosition.z + Random.Range(-freeSpaceZ, freeSpaceZ));
        }
    }
}
