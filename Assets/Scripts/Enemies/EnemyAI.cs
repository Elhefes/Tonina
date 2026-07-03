using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAI : MonoBehaviour
{
    public Creature enemyCreature;
    public Weapon meleeWeapon;
    public Weapon rangedWeapon;
    public float rangedWeaponRangeLimit;

    void OnEnable()
    {
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
        if (meleeWeapon == null || rangedWeapon == null) return;
        Transform target = enemyCreature.creatureMovement.target;
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > rangedWeaponRangeLimit)
        {
            if (!rangedWeapon.gameObject.activeSelf) SwitchToRangedWeapon();
        }
        else if (distance < rangedWeaponRangeLimit)
        {
            if (!meleeWeapon.gameObject.activeSelf) SwitchToMeleeWeapon();
        }
    }

    public void SwitchToRangedWeapon()
    {
        meleeWeapon.gameObject.SetActive(false);
        rangedWeapon.gameObject.SetActive(true);
        enemyCreature.weaponOnHand = rangedWeapon;
        enemyCreature.SetWeaponBarricadeCollisionHandling();
    }

    public void SwitchToMeleeWeapon()
    {
        rangedWeapon.gameObject.SetActive(false);
        meleeWeapon.gameObject.SetActive(true);
        enemyCreature.weaponOnHand = meleeWeapon;
        enemyCreature.SetWeaponBarricadeCollisionHandling();
    }
}