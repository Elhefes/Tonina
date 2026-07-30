using System.Collections;
using UnityEngine;

public class Camazo : MonoBehaviour
{
    public bool activeInBattle;
    public ParticleSystem attackParticleSystem;
    public AudioSource audioSource;
    public AudioClip incomingSound;
    public AudioClip attackSound;
    public float incomingSoundMaxRange;
    public float attackSoundMaxRange;
    public Player player;

    [Header("Waypoints")]
    public Transform pointA;
    public Transform pointB; // "Default target point"
    public Transform targetPoint;
    public Transform pointC;

    [Header("Movement")]
    public float moveSpeed;
    public float rotationSpeed;
    public float waypointReachedDistance;
    public float finalApproachDistance;

    [Header("Sphere Attack")]
    public float sphereAttackRadius;
    public float sphereAttackMaxDamage;

    private Vector3 targetVerticalOffset = new(0f, 2.75f, 0f);
    private bool movingTowardsPosition;

    private void OnEnable()
    {
        if (pointA != null) transform.position = pointA.position;
        if (pointA == null || pointB == null || pointC == null) return;
        targetPoint = pointB;
        if (activeInBattle)
        {
            StartCoroutine(FlyLoop());
            StartCoroutine(SearchForTargetLoop());
        }
    }

    private IEnumerator FlyLoop()
    {
        while (activeInBattle)
        {
            yield return new WaitForSeconds(DynamicWaitTime());

            yield return MoveToTargetPoint();
            yield return MoveTo(pointC);

            yield return new WaitForSeconds(DynamicWaitTime());

            yield return MoveToTargetPoint();
            yield return MoveTo(pointA);
        }
    }

    private IEnumerator SearchForTargetLoop()
    {
        while(activeInBattle)
        {
            yield return new WaitForSeconds(1.5f);
            if (targetPoint == null) targetPoint = pointB;
            if (Vector3.Distance(transform.position, targetPoint.position) > finalApproachDistance
                && movingTowardsPosition) TargetNearestEnemyToPlayer();
        }
    }

    private float DynamicWaitTime()
    {
        float waitTime = 6.666f;
        if (player.health > player.startingHealth * 0.5f) waitTime += 6.666f;
        if (player.health >= player.startingHealth) waitTime += 9f;
        return waitTime;
    }

    private IEnumerator MoveTo(Transform target)
    {
        movingTowardsPosition = true;

        while (Vector3.Distance(transform.position, target.position) > waypointReachedDistance)
        {
            // Direction to destination
            Vector3 direction = (target.position - transform.position).normalized;

            // Smoothly turn toward it
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime);
            }

            // Fly forward instead of directly toward the target
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            yield return null;
        }

        // Snap exactly to the waypoint
        transform.position = target.position;

        movingTowardsPosition = false;
    }

    private IEnumerator MoveToTargetPoint() // This exists because targetPoint changes constantly; single call is no problem
    {
        movingTowardsPosition = true;

        audioSource.maxDistance = incomingSoundMaxRange;
        audioSource.volume = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        audioSource.PlayOneShot(incomingSound);

        if (targetPoint == null) targetPoint = pointB;

        while (Vector3.Distance(transform.position, targetPoint.position + targetVerticalOffset) > waypointReachedDistance)
        {
            // Direction to destination
            Vector3 direction = (targetPoint.position + targetVerticalOffset - transform.position).normalized;

            // Smoothly turn toward it
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime);
            }

            // Fly forward instead of directly toward the target
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            yield return null;
        }

        // Snap exactly to the waypoint
        transform.position = targetPoint.position + targetVerticalOffset;

        SphereAttack();

        movingTowardsPosition = false;
    }

    private void SphereAttack()
    {
        attackParticleSystem.Play();

        audioSource.maxDistance = attackSoundMaxRange;
        audioSource.volume = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        audioSource.PlayOneShot(attackSound);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereAttackRadius);

        foreach (Collider hit in hitColliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null)
                continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // 1 at the center, 0 at the edge
            float t = Mathf.Clamp01(1f - (distance / sphereAttackRadius));

            // Damage falls off linearly
            int damage = Mathf.RoundToInt(sphereAttackMaxDamage * t);

            if (damage > 0)
            {
                enemy.SlowDownEnemy();
                enemy.TakeDamage(damage);
            }
        }
    }

    private void TargetNearestEnemyToPlayer()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity;
        foreach (GameObject obj in objectsWithTag)
        {
            if (!obj.activeInHierarchy) continue; // Ignore disabled objects
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < nearestDistance)
            {
                nearestObject = obj;
                nearestDistance = distance;
            }
        }
        if (nearestObject != null)
        {
            targetPoint = nearestObject.transform;
        }
    }
}