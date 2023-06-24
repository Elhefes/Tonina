using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float jumpHeight;
    public float rotationSpeed;
    private bool isMoving;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public NavMeshAgent agent;

    public Transform shootingPoint;
    public GameObject arrow;

    public float projectileForce;

    public Animator playerAnimator;
    public Animator playerLegsAnimator;

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
                isMoving = true;
                playerAnimator.SetBool("IsMoving", true);
                playerLegsAnimator.SetBool("IsMoving", true);
                agent.destination = hit.point;
            }
        }

        if (!isMoving && agent.remainingDistance <= agent.stoppingDistance)
        {
            isMoving = false;
            playerAnimator.SetBool("IsMoving", false);
            playerLegsAnimator.SetBool("IsMoving", false);
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
}