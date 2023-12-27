using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CreatureMovement : MonoBehaviour
{
    public CharacterController controller;

    public float jumpHeight;
    public float rotationSpeed;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public NavMeshAgent agent;

    public Animator animator;

    public Camera cam;

    public Transform target;

    public Player player;

    public Enemy enemy;

    void Update()
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

    void LegsTimingTrigger()
    {
        animator.SetTrigger("LegsTiming");
    }

    void ResetTrigger(string trigger)
    {
        animator.ResetTrigger(trigger);
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