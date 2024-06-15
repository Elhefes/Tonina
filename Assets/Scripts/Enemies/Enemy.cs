using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Creature
{
    public NavMeshAgent agent;
    public Slider healthBar;
    public int startingHealth;
    private int health;
    public bool moveTowardsTarget;
    public float attackExtraCooldown;
    private Coroutine slowDownCoroutine;
    private bool slowedDown;
    public float slowDownTimeFromStone;
    public float speedDropFromStone;

    void Awake()
    {
        if (moveTowardsTarget) creatureMovement.target = GameObject.FindGameObjectsWithTag("Target")[0].transform;
        health = startingHealth;
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

    public void SlowDownEnemy()
    {
        if (slowedDown)  return;
        slowDownCoroutine = StartCoroutine(SlowDownCoroutine());
    }

    private IEnumerator SlowDownCoroutine()
    {
        float originalSpeed = agent.speed;
        slowedDown = true;
        agent.speed -= speedDropFromStone;
        yield return new WaitForSeconds(slowDownTimeFromStone / 2);
        agent.speed += speedDropFromStone / 2;
        yield return new WaitForSeconds(slowDownTimeFromStone / 2);
        agent.speed = originalSpeed;
        slowedDown = false;
    }
}