using System.Collections;
using UnityEngine;

public class Camazo : MonoBehaviour
{
    public bool activeInBattle;
    public ParticleSystem attackParticleSystem;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip incomingSound;
    public AudioClip attackSound;
    public float incomingSoundMaxRange;
    public float attackSoundMaxRange;
    public Player player;

    public Vector3 evolutionScale1 = new(0.08f, 0.08f, 0.08f);
    public Vector3 evolutionScale2 = new(0.11f, 0.11f, 0.11f);
    public Vector3 evolutionScale3 = new(0.14f, 0.14f, 0.14f);
    public Vector3 evolutionScale4 = new(0.18f, 0.18f, 0.18f);

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
        ResetAnimatorStates();
        if (pointA != null) transform.position = pointA.position;
        if (activeInBattle) SetActiveInBattle();
    }

    public void SetEvolutionStage()
    {
        int bonesEaten = GameState.Instance.progressionData.bonesCamazoHasEaten;
        if (bonesEaten > 67)
        {
            GameState.Instance.progressionData.camazoEvolutionStage = 3;
            GameState.Instance.progressionData.camazoIsActive = true;
        }
        else if (bonesEaten > 37) GameState.Instance.progressionData.camazoEvolutionStage = 2;
        else if (bonesEaten > 15) GameState.Instance.progressionData.camazoEvolutionStage = 1;
        else GameState.Instance.progressionData.camazoEvolutionStage = 0;
    }

    public void LoadEvolutionScale()
    {
        int evolutionStage = GameState.Instance.progressionData.camazoEvolutionStage;
        if (evolutionStage == 0)
        {
            transform.localScale = evolutionScale1;
            animator.SetInteger("EvolutionStage", 1);
        }
        else if (evolutionStage == 1)
        {
            transform.localScale = evolutionScale2;
            animator.SetInteger("EvolutionStage", 2);
        }
        else if (evolutionStage == 2)
        {
            transform.localScale = evolutionScale3;
            animator.SetInteger("EvolutionStage", 3);
        }
        else
        {
            transform.localScale = evolutionScale4;
            animator.SetInteger("EvolutionStage", 4);
        }
    }

    public void ResetAnimatorStates()
    {
        animator.SetBool("Flapping", false);
        animator.SetBool("Hanging", false);
    }

    public void SetFlapping()
    {
        ResetAnimatorStates();
        animator.SetBool("Flapping", true);
    }

    public void SetHanging()
    {
        ResetAnimatorStates();
        animator.SetBool("Hanging", true);
    }

    public void SetActiveInBattle()
    {
        if (pointA == null || pointB == null || pointC == null) return;
        activeInBattle = true;
        targetPoint = pointB;
        SetFlapping();
        StartCoroutine(FlyLoop());
        StartCoroutine(SearchForTargetLoop());
    }

    private IEnumerator FlyLoop()
    {
        while (activeInBattle)
        {
            yield return new WaitForSeconds(DynamicWaitTime());

            yield return MoveToTargetPoint();
            yield return MoveTo(pointC);

            yield return new WaitForSeconds(DynamicWaitTime());

            if (!activeInBattle) break;

            yield return MoveToTargetPoint();
            yield return MoveTo(pointA);
        }
    }

    private IEnumerator SearchForTargetLoop()
    {
        while (activeInBattle)
        {
            yield return new WaitForSeconds(1.5f);

            if (targetPoint == null)
            {
                targetPoint = pointB;
                continue;
            }

            if (Vector3.Distance(
                    transform.position,
                    targetPoint.position) > finalApproachDistance
                && movingTowardsPosition)
            {
                TargetNearestEnemyToPlayer();
            }
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

        if (targetPoint == null)
        {
            movingTowardsPosition = false;
            yield break;
        }

        while (true)
        {
            // If the enemy was destroyed, return to pointB
            if (targetPoint == null)
            {
                targetPoint = pointB;

                if (targetPoint == null)
                {
                    movingTowardsPosition = false;
                    yield break;
                }
            }

            Vector3 destination = targetPoint.position + targetVerticalOffset;

            // Check whether we have reached the target
            if (Vector3.Distance(transform.position, destination)
                <= waypointReachedDistance)
            {
                break;
            }

            // Direction to destination
            Vector3 direction = (destination - transform.position).normalized;

            // Smoothly turn toward it
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            // Fly forward instead of directly toward the target
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            yield return null;
        }

        // Snap to the final destination
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
            if (enemy == null) continue;

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
        if (player == null) return;

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