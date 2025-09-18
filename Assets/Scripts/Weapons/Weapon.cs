using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public bool isFriendly;
    public bool selected;
    public bool notAvailable;
    public WeaponType type;
    public int damage;
    public Sprite uiSprite;
    public float attackCooldown = 1.0f; // Time between attacks
    public float attackDistance;
    public float attackAngleThreshold;
    public bool canHit;
    public AudioClip switchSound;
    public AudioClip hitSound;
    public AudioSource soundPlayer;

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

    public void HandleCollision(GameObject obj)
    {
        if (!canHit) return;
        if (isFriendly && obj.CompareTag("Enemy") && !hitEnemies.Contains(obj))
        {
            hitEnemies.Add(obj);
            obj.GetComponent<Enemy>()?.TakeDamage(damage);
            if (hitSound != null) soundPlayer.PlayOneShot(hitSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));
        }
        if (!isFriendly && (obj.CompareTag("Player") || obj.CompareTag("ToninaTribe") || obj.CompareTag("Barricade")) && !hitEnemies.Contains(obj))
        {
            hitEnemies.Add(obj);
            obj.GetComponent<Player>()?.TakeDamage(damage);
            obj.GetComponent<ToninaWarrior>()?.TakeDamage(damage);
            obj.GetComponent<Barricade>()?.TakeDamage(damage);
            if (hitSound != null) soundPlayer.PlayOneShot(hitSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));
        }
    }
}

public enum WeaponType
{
    Club,
    Axe,
    SmallStone,
    Spear,
    Bow
}