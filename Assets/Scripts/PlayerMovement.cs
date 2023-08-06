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

    public Transform shootingPoint;
    public GameObject arrow;

    public float projectileForce;

    public Animator playerAnimator;

    public Camera cam;

    private Transform movingTarget;

    public float attackDistance;

    private Coroutine attackCoroutine;

    private EnemyAI enemy;

    void Update()
    {
        if (movingTarget)
        {
            agent.destination = movingTarget.position;
            if (Vector3.Distance(transform.position, movingTarget.position) <= attackDistance) 
            {
                if (attackCoroutine == null) attackCoroutine = StartCoroutine(Attack());
            } 
            else
            {
                if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        } 
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (attackCoroutine != null) StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            movingTarget = null;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                GameObject target = hit.collider.gameObject;
                if (target.CompareTag("Enemy"))
                {
                    enemy = target.GetComponent<EnemyAI>();
                    movingTarget = target.transform;
                }
                agent.SetDestination(hit.point);
            }
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

    IEnumerator Attack()
    {
        while (enemy)
        {
            MeleeAttack();
            yield return new WaitForSeconds(1);
        }
    }

    void FireProjectile()
    {
        GameObject projectile = Instantiate(arrow, shootingPoint.position, shootingPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileForce, ForceMode.Impulse);
    }

    void MeleeAttack()
    {
        playerAnimator.SetTrigger("ClubAttack");
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