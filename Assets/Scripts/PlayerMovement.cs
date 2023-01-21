using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float jumpHeight;
    public float walkingSpeed;
    public float runningSpeed;
    private float speed;
    public float rotationSpeed;
    private float gravity = -9.81f;
    private bool isMoving;
    private Vector3 targetPosition;
    private Vector3 moveDirection;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public NavMeshAgent agent;

    public Transform shootingPoint;
    public GameObject arrow;

    public float projectileForce;

    Vector3 velocity;
    bool isGrounded;

    public Camera cam;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Get the location of the mouse click
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                isMoving = true;
                agent.destination = hit.point;
            }
        }

        if (!isMoving && agent.remainingDistance <= agent.stoppingDistance)
        {
            isMoving = false;
        }

        if (Input.GetKey("left shift") && isGrounded)
        {
            speed = runningSpeed;
        }
        else
        {
            speed = walkingSpeed;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        GameObject projectile = Instantiate(arrow, shootingPoint.position, shootingPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileForce, ForceMode.Impulse);
    }
}