using UnityEngine;
using UnityEngine.AI;

public class CreatureMovement : MonoBehaviour
{
    public CharacterController controller;

    public float jumpHeight;
    public float rotationSpeed;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public NavMeshAgent agent;
    private NavMeshPath path;
    public Animator animator;
    public Transform target;

    private void Start()
    {
        path = new NavMeshPath();
    }

    public void Update()
    {
        if (target)
        {
            agent.destination = target.position;
        }
        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
        }

        if (agent.velocity == Vector3.zero || (agent.remainingDistance <= agent.stoppingDistance))
        {
            animator.SetBool("IsMoving", false);
        }
    }

    public void MoveToDestination(Vector3 destination)
    {
        NavMesh.CalculatePath(agent.transform.position, destination, groundMask, path);

        if (path.status == NavMeshPathStatus.PathComplete ||
            path.status == NavMeshPathStatus.PathPartial)
        {
            agent.SetPath(path);
        }
        else agent.SetDestination(destination);
        agent.destination = destination;
    }

    void LegsTimingTrigger()
    {
        animator.SetTrigger("LegsTiming");
    }

    void ResetTrigger(string trigger)
    {
        animator.ResetTrigger(trigger);
    }
}