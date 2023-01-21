using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;

    void Update()
    {
        agent.destination = player.position;
    }
}