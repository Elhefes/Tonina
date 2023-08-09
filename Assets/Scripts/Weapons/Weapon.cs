using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public WeaponType type;
    public int damage;
    public Sprite uiSprite;
    public float attackCooldown = 1.0f; // Time between attacks
    public float attackDistance;
    public float attackAngleThreshold;
    public bool canHit;

    private List<GameObject> hitEnemies = new List<GameObject>();

    public virtual void Attack(Animator animator)
    {
            
    }

    public bool ShouldAttack(float distanceToTarget, float angle)
    {
        // Move the attack condition logic here
        return distanceToTarget <= attackDistance && angle < attackAngleThreshold;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        var obj = collision.gameObject;
        if (hitEnemies.Contains(obj))
        {
            hitEnemies.Remove(obj);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other.gameObject);
    }

    private void HandleCollision(GameObject obj)
    {
        if (!canHit) return;
        if (obj.CompareTag("Enemy") && !hitEnemies.Contains(obj))
        {
            hitEnemies.Add(obj);
            obj.GetComponent<EnemyAI>()?.TakeDamage(damage);
        }
    }
}

public enum WeaponType
{
    Club,
    Small_stone,
    Spear
}