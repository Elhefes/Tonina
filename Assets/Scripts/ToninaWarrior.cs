using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ToninaWarrior : Creature
{
    public NavMeshAgent agent;
    public Slider healthBar;
    public int startingHealth;
    private int health;
    public GameObject deathSoundObject;
    public bool moveTowardsTarget;
    public float attackExtraCooldown;

    void Awake()
    {
        health = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        healthBar.value -= (float)damage / startingHealth;
        health -= damage;
        if (health <= 0)
        {
            if (deathSoundObject != null)
            {
                deathSoundObject.SetActive(true);
                deathSoundObject.transform.SetParent(null);
            }
            Destroy(gameObject);
        }
        print(health);
    }
}
