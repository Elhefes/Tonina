using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ToninaWarrior : Creature
{
    public string friendlyType;
    public NavMeshAgent agent;
    public Slider healthBar;
    public int startingHealth;
    private int health;
    public GameObject deathSoundObject;
    public bool moveTowardsTarget;
    public float attackExtraCooldown;

    public ObjectPooler pooler;
    protected bool friendlyInPool = true;

    void Awake()
    {
        health = startingHealth;
        pooler = ObjectPooler.Instance;
    }

    public void ResetFriendlyAttributes()
    {
        health = startingHealth;
        healthBar.value = health;
    }

    public void SetFriendlyInPool(bool value)
    {
        friendlyInPool = value;
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
            if (friendlyInPool)
            {
                pooler.AddFriendlyToPool(this, friendlyType);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        print(health);
    }
}
