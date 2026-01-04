using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Creature
{
    public string enemyType;
    public NavMeshAgent agent;
    public Slider healthBar;
    public int startingHealth;
    private int health;
    public GameObject deathSoundObject;
    public bool moveTowardsTarget;
    private Coroutine slowDownCoroutine;
    public float movementSpeed;
    private bool slowedDown;
    public float slowDownTimeFromStone;
    public float speedDropFromStone;

    public ObjectPooler pooler;
    protected bool enemyInPool = true;

    void Awake()
    {
        pooler = ObjectPooler.Instance;
        ResetEnemyAttributes();
        FindFirstTarget();
    }

    void FindFirstTarget()
    {
        GameObject[] foundTargets = GameObject.FindGameObjectsWithTag("Target");

        if (foundTargets.Length > 0)
        {
            if (moveTowardsTarget) creatureMovement.target = foundTargets[0].transform;
        }
    }

    public void ResetEnemyAttributes()
    {
        health = startingHealth;
        healthBar.value = health;
        slowedDown = false;
        StopAllCoroutines();
        agent.speed = movementSpeed;
    }

    public void TakeDamage(int damage)
    {
        healthBar.value -= (float) damage / startingHealth;
        health -= damage;
        if (health <= 0)
        {
            if (deathSoundObject != null)
            {
                deathSoundObject.SetActive(true);
                deathSoundObject.transform.SetParent(null);
            }
            if (pooler != null && enemyInPool)
            {
                pooler.AddEnemyToPool(this, enemyType);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        print(health);
    }

    public void SetEnemyInPool(bool value)
    {
        enemyInPool = value;
    }

    public void SlowDownEnemy()
    {
        if (slowedDown || !gameObject.activeSelf) return;
        slowDownCoroutine = StartCoroutine(SlowDownCoroutine());
    }

    private IEnumerator SlowDownCoroutine()
    {
        slowedDown = true;
        agent.speed -= speedDropFromStone;
        yield return new WaitForSeconds(slowDownTimeFromStone / 2);
        agent.speed += speedDropFromStone / 2;
        yield return new WaitForSeconds(slowDownTimeFromStone / 2);
        agent.speed = movementSpeed;
        slowedDown = false;
    }
}