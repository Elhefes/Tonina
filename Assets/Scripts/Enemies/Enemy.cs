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
    private bool slowedDown;
    public float slowDownTimeFromStone;
    public float speedDropFromStone;

    public ObjectPooler pooler;
    protected bool enemyInPool = true;

    void Awake()
    {
        pooler = ObjectPooler.Instance;
        if (moveTowardsTarget) creatureMovement.target = GameObject.FindGameObjectsWithTag("Target")[0].transform;
        ResetEnemyAttributes();
    }

    public void ResetEnemyAttributes()
    {
        health = startingHealth;
        healthBar.value = health;
        slowedDown = false;
        StopAllCoroutines();
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
            if (enemyInPool)
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