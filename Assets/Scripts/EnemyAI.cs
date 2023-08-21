using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Slider healthBar;
    public int startingHealth;
    private int health;
    public bool moveTowardsTarget;

    void Awake()
    {
        if (moveTowardsTarget) agent.destination = GameObject.FindGameObjectsWithTag("Target")[0].transform.position;
        health = startingHealth;
    }

    void Update()
    {
        if (player == null) return;
        agent.destination = player.position;
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