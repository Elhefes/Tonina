using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class JadeaWarrior : Creature
{
    public string friendlyType;
    public NavMeshAgent agent;
    public Slider healthBar;
    public int defaultStartingHealth;
    private int startingHealth;
    private int health;
    public float defaultSpeed;
    public GameObject deathSoundObject;
    public bool moveTowardsTarget;
    public float attackExtraCooldown;

    protected bool friendlyInPool = true;

    void Start()
    {
        startingHealth = defaultStartingHealth +
            PlayerAttributes.LeadershipHealthBuff(GameState.Instance.progressionData.leadershipLevel);
        health = startingHealth;
        agent.speed = defaultSpeed +
            PlayerAttributes.LeadershipSpeedBuff(GameState.Instance.progressionData.leadershipLevel);
        pooler = ObjectPooler.Instance;
        attackerSideSetting = AttackerSideSetting.Instance;
        if (attackerSideSetting != null) isAttacker = attackerSideSetting.enemyIsDefender;
    }

    public void ResetFriendlyAttributes()
    {
        startingHealth = defaultStartingHealth +
            PlayerAttributes.LeadershipHealthBuff(GameState.Instance.progressionData.leadershipLevel);
        health = startingHealth;
        healthBar.value = health;
        agent.speed = defaultSpeed +
            PlayerAttributes.LeadershipSpeedBuff(GameState.Instance.progressionData.leadershipLevel);
        creatureMovement.target = null;
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
            if (pooler != null && friendlyInPool)
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
