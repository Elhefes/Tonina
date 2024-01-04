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
}