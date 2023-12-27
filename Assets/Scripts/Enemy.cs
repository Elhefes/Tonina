using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Creature
{
    public Transform player;
    public NavMeshAgent agent;
    public Slider healthBar;
    public Animator enemyAnimator;
    private Coroutine attackCoroutine;
    public int startingHealth;
    private int health;
    public bool moveTowardsTarget;
    public float attackExtraCooldown;

    void Awake()
    {
        if (moveTowardsTarget) agent.destination = GameObject.FindGameObjectsWithTag("Target")[0].transform.position;
        health = startingHealth;
    }

    void Update()
    {
        if (player == null) return;
        agent.destination = player.position;

        if (agent.destination != null)
        {
            var directionToTarget = player.position - transform.position;
            directionToTarget.y = 0;
            var angle = Vector3.Angle(transform.forward, directionToTarget);
            Debug.DrawLine(transform.position, transform.position + directionToTarget * 114f, Color.red);

            // Move the attack condition logic to the Weapon class
            bool shouldAttack = weaponOnHand.ShouldAttack(Vector3.Distance(transform.position, player.position), angle);

            if (shouldAttack)
            {
                if (attackCoroutine == null) attackCoroutine = StartCoroutine(Attack());
            }
            else
            {
                if (Vector3.Distance(transform.position, player.position) <= agent.stoppingDistance + 1f)
                {
                    // Calculate the direction to the destination
                    Vector3 direction = (agent.destination - transform.position).normalized;

                    // Ignore the y-axis to prevent tilting
                    direction.y = 0f;

                    // Rotate towards the destination
                    if (direction != Vector3.zero)
                    {
                        Quaternion toRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * agent.angularSpeed * 0.25f);
                    }
                }
                if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private IEnumerator Attack()
    {
        while (player != null)
        {
            weaponOnHand.Attack(enemyAnimator);
            print("ATTACK!");
            yield return new WaitForSeconds(weaponOnHand.attackCooldown + attackExtraCooldown);
        }
    }

    public void TakeDamage(int damage)
    {
        healthBar.value -= (float) damage / startingHealth;
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        print(health);
    }

    void LegsTimingTrigger()
    {
        enemyAnimator.SetTrigger("LegsTiming");
    }

    void ResetTrigger(string trigger)
    {
        enemyAnimator.ResetTrigger(trigger);
    }
}