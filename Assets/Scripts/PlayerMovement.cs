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

    public Player player;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MaizePlace"))
        {
            player.maizePlace = other.GetComponentInParent<MaizePlace>();
            player.EnterMaizePlace();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MaizePlace"))
        {
            player.ExitMaizePlace();
        }
    }
}