using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAI : MonoBehaviour
{
    public Creature enemyCreature;
    public Weapon meleeWeapon;
    public Projectile rangedWeapon;
    public int rangedWeaponQuantity;
    public float rangedWeaponRangeLimit;

    public float weaponSwitchCoolDownTime;
    private bool weaponSwitchCooldown;

    // Alternates which spawned/enabled friendly is allowed to carry a ranged
    // weapon. Odd/even spawn order, shared across all FriendlyAI instances.
    public bool everyOtherHasRangedWeapon;
    private static int spawnCount;
    private bool hasRangedWeapon;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetSpawnSequence()
    {
        spawnCount = 0;
    }

    void OnEnable()
    {
        if (rangedWeapon != null)
        {
            // Melee is default weapon
            enemyCreature.creatureMovement.animator.SetBool("RangedEquipped", false);
            enemyCreature.creatureMovement.animator.SetBool("MeleeEquipped", true);
            rangedWeapon.gameObject.SetActive(false);
            rangedWeapon.quantity = rangedWeaponQuantity;
            rangedWeapon.notAvailable = false;

            if (everyOtherHasRangedWeapon)
            {
                hasRangedWeapon = spawnCount % 2 == 0;
                spawnCount++;
            }
            else hasRangedWeapon = true;
        }
        if (meleeWeapon != null)
        {
            meleeWeapon.gameObject.SetActive(true);
            enemyCreature.weaponOnHand = meleeWeapon;
        }

        StartCoroutine(PeriodicalTargetChecking());
        enemyCreature.SetWeaponBarricadeCollisionHandling();
    }

    void Update()
    {
        UpdateWeaponSelection();
    }

    protected virtual IEnumerator PeriodicalTargetChecking()
    {
        float waitTime = 3f;
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            if (NearestTarget(CreatureTargets.jadeaWarriors, 8f) != null)
            {
                waitTime = 0.5f;
            }
            else if (enemyCreature.isAttacker && NearestTarget("Barricade", 10f) != null)
            {
                waitTime = 1f;
            }
            else if (NearestTarget(CreatureTargets.jadeaWarriors, 20f) != null)
            {
                waitTime = 4f;
            }
            else
            {
                GameObject[] g = GameObject.FindGameObjectsWithTag("Target");
                if (g.Length > 0) enemyCreature.creatureMovement.target = g[0].transform;
                waitTime = 3f;
            }
        }
    }

    public GameObject NearestTarget(string tag, float range)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity;
        foreach (GameObject obj in objectsWithTag)
        {
            if (!obj.activeInHierarchy) continue; // Ignore disabled objects
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < nearestDistance && distance <= range)
            {
                nearestObject = obj;
                nearestDistance = distance;
            }
        }
        if (nearestObject != null) enemyCreature.creatureMovement.target = nearestObject.transform;
        return nearestObject;
    }

    // Same as above, but scans a CreatureTargets list (e.g. jadeaWarriors) instead
    // of doing a FindGameObjectsWithTag call every time.
    public GameObject NearestTarget(List<Creature> creatures, float range)
    {
        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity;
        foreach (Creature creature in creatures)
        {
            if (creature == null || !creature.gameObject.activeInHierarchy) continue; // Ignore disabled objects
            float distance = Vector3.Distance(transform.position, creature.transform.position);
            if (distance < nearestDistance && distance <= range)
            {
                nearestObject = creature.gameObject;
                nearestDistance = distance;
            }
        }
        if (nearestObject != null) enemyCreature.creatureMovement.target = nearestObject.transform;
        return nearestObject;
    }

    private void UpdateWeaponSelection()
    {
        if (hasRangedWeapon)
        {
            if (rangedWeapon.notAvailable && !meleeWeapon.gameObject.activeSelf) SwitchToMeleeWeapon();
        }
        if (enemyCreature.onCooldown || weaponSwitchCooldown || !hasRangedWeapon) return;
        if (meleeWeapon == null || rangedWeapon == null) return;
        Transform target = enemyCreature.creatureMovement.target;
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < rangedWeaponRangeLimit)
        {
            // Always throw the last ranged weapon, especially good for SpearWarrior
            if (!meleeWeapon.gameObject.activeSelf && rangedWeapon.quantity > 1) SwitchToMeleeWeapon();
        }
        else
        {
            if (!rangedWeapon.notAvailable && !rangedWeapon.gameObject.activeSelf) SwitchToRangedWeapon();
        }
    }

    public void SwitchToRangedWeapon()
    {
        enemyCreature.weaponOnHand = rangedWeapon;
        enemyCreature.weaponOnHand.canHit = false;
        meleeWeapon.gameObject.SetActive(false);
        rangedWeapon.gameObject.SetActive(true);
        enemyCreature.SetWeaponBarricadeCollisionHandling();

        enemyCreature.creatureMovement.animator.SetBool("MeleeEquipped", false);
        enemyCreature.creatureMovement.animator.SetBool("RangedEquipped", true);

        StartWeaponSwitchCooldown();
    }

    public void SwitchToMeleeWeapon()
    {
        enemyCreature.weaponOnHand = meleeWeapon;
        enemyCreature.weaponOnHand.canHit = false;
        rangedWeapon.gameObject.SetActive(false);
        meleeWeapon.gameObject.SetActive(true);
        enemyCreature.SetWeaponBarricadeCollisionHandling();

        enemyCreature.creatureMovement.animator.SetBool("RangedEquipped", false);
        enemyCreature.creatureMovement.animator.SetBool("MeleeEquipped", true);

        StartWeaponSwitchCooldown();
    }

    void StartWeaponSwitchCooldown()
    {
        Invoke("ResetWeaponSwitchCooldown", weaponSwitchCoolDownTime);
        weaponSwitchCooldown = true;
    }

    void ResetWeaponSwitchCooldown()
    {
        weaponSwitchCooldown = false;
    }
}