using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float jumpHeight;
    public float rotationSpeed;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public NavMeshAgent agent;

    public Animator playerAnimator;

    public Camera cam;

    public Transform target;


    public EnemyAI enemy;

    void Update()
    {
        if (target)
        {
            agent.destination = target.position;
        }
        if (agent.velocity != Vector3.zero)
        {
            playerAnimator.SetBool("IsMoving", true);
        }

        if (agent.velocity == Vector3.zero || (agent.remainingDistance <= agent.stoppingDistance))
        {
            playerAnimator.SetBool("IsMoving", false);
        }
    }

    void LegsTimingTrigger()
    {
        playerAnimator.SetTrigger("LegsTiming");
    }

    void ResetTrigger(string trigger)
    {
        playerAnimator.ResetTrigger(trigger);
    }
}