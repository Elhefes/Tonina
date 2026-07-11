using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public bool isFriendly;
    public bool canHitBarricades;
    public bool selected;
    public bool notAvailable;
    public WeaponType type;
    public int defaultDamage;
    public int damage;
    public bool recievesWeaponsBuffs;
    public bool recievesLeadershipBuffs;
    public Sprite uiSprite;
    public float defaultAttackCooldown; // Time between attacks
    public float attackCooldown;
    public float attackDistance;
    public float attackAngleThreshold;
    public bool canHit;
    public AudioClip switchSound;
    public AudioClip hitSound;
    public AudioSource soundPlayer;

    private List<GameObject> hitObjects = new List<GameObject>();

    private void OnEnable()
    {
        UpdateWeaponValues();
        hitObjects.Clear();
    }

    public virtual void Attack(Animator animator)
    {
            
    }

    public virtual void UpdateWeaponValues()
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
        if (hitObjects.Contains(obj))
        {
            hitObjects.Remove(obj);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other.gameObject);
    }

    public void HandleCollision(GameObject obj)
    {
        if (!canHit) return;
        if (isFriendly && obj.CompareTag("Enemy") && !hitObjects.Contains(obj))
        {
            hitObjects.Add(obj);
            obj.GetComponent<Enemy>()?.TakeDamage(damage);

            if (hitSound != null) soundPlayer.PlayOneShot(hitSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));
        }
        if (!isFriendly && (obj.CompareTag("Player") || obj.CompareTag("Jadea")) && !hitObjects.Contains(obj))
        {
            hitObjects.Add(obj);
            obj.GetComponent<Player>()?.TakeDamage(damage);
            obj.GetComponent<JadeaWarrior>()?.TakeDamage(damage);
            
            if (hitSound != null) soundPlayer.PlayOneShot(hitSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));
        }
        if (obj.CompareTag("Barricade") && !hitObjects.Contains(obj))
        {
            hitObjects.Add(obj);
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