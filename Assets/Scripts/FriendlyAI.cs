using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FriendlyAI : MonoBehaviour
{
    public Creature friendlyCreature;
    public Weapon meleeWeapon;
    public Projectile rangedWeapon;
    public int rangedWeaponQuantity;
    public float rangedWeaponRangeLimit;

    public float weaponSwitchCoolDownTime;
    private bool weaponSwitchCooldown;

    private Player player;
    private float normalStoppingDistance;

    // Alternates which spawned/enabled friendly is allowed to carry a ranged
    // weapon. Odd/even spawn order, shared across all FriendlyAI instances.
    private static int spawnCount;
    private bool hasRangedWeapon;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetSpawnSequence()
    {
        spawnCount = 0;
    }

    private bool isGuarding;
    private Vector3 guardAnchor;
    private float guardStepIssuedTime;
    private const float guardRadius = 8f;
    private const float guardTriggerDistance = 40f;

    private bool isSteppingAside;
    private float stepIssuedTime;
    private const float makeWayTriggerDistance = 6f;
    // Once stepping aside, we don't resume normal following until we're past this
    // wider distance - a straight >6f check would flip back and forth every tick
    // right around the trigger line, since a single 3f step barely clears it.
    private const float makeWayClearDistance = 8f;
    private const float makeWayBackDistance = 3f;
    // Friendlies physically collide with each other, so a retreat step can get
    // blocked by a neighbor and never technically "arrive". If that happens, give
    // up waiting and try a fresh (re-randomized) step instead of standing stuck.
    private const float stepTimeout = 1f;

    void Awake()
    {
        player = FindFirstObjectByType<Player>();
        normalStoppingDistance = friendlyCreature.creatureMovement.agent.stoppingDistance;
    }
    private void OnEnable()
    {
        if (player == null) player = FindFirstObjectByType<Player>(); // In attack mode
        
        isGuarding = false;
        isSteppingAside = false;

        // Melee is default weapon
        friendlyCreature.creatureMovement.animator.SetBool("RangedEquipped", false);
        friendlyCreature.creatureMovement.animator.SetBool("MeleeEquipped", true);

        if (rangedWeapon != null)
        {
            rangedWeapon.gameObject.SetActive(false);
            rangedWeapon.quantity = rangedWeaponQuantity;
            rangedWeapon.notAvailable = false;
        }
        else if (meleeWeapon != null)
        {
            meleeWeapon.gameObject.SetActive(true);
            friendlyCreature.weaponOnHand = meleeWeapon;
        }

        hasRangedWeapon = spawnCount % 2 == 0;
        spawnCount++;

        StartCoroutine(PeriodicalTargetChecking());
        friendlyCreature.SetWeaponBarricadeCollisionHandling();
    }

    void Update()
    {
        UpdateWeaponSelection();
    }

    private IEnumerator PeriodicalTargetChecking()
    {
        float waitTime = Random.Range(2f, 3f);
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            friendlyCreature.creatureMovement.agent.stoppingDistance = normalStoppingDistance;
            if (NearestTarget(CreatureTargets.enemies, 8f) != null)
            {
                waitTime = 0.5f;
                isGuarding = false;
                isSteppingAside = false;
            }
            else if (friendlyCreature.isAttacker && NearestTarget("Barricade", 10f) != null)
            {
                waitTime = 1f;
                isGuarding = false;
                isSteppingAside = false;
            }
            else if (NearestTarget(CreatureTargets.enemies, 30f) != null)
            {
                waitTime = 4f;
                isGuarding = false;
                isSteppingAside = false;
            }
            else if (player != null && Vector3.Distance(transform.position, player.transform.position) > guardTriggerDistance)
            {
                isSteppingAside = false;
                Guard();
                waitTime = Random.Range(2.5f, 5f);
            }
            else
            {
                isGuarding = false;
                FollowPlayerDirections();
                waitTime = Random.Range(0.5f, 1f);
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
        if (nearestObject != null)
        {
            friendlyCreature.creatureMovement.target = nearestObject.transform;
        }
        return nearestObject;
    }

    // Same as above, but scans a CreatureTargets list (e.g. enemies) instead
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
        if (nearestObject != null)
        {
            friendlyCreature.creatureMovement.target = nearestObject.transform;
        }
        return nearestObject;
    }

    void FollowPlayerDirections()
    {
        friendlyCreature.creatureMovement.target = null;
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float distanceToPlayerDestination = Vector3.Distance(transform.position, player.destination);

        // Hysteresis: enter make-way mode at the tight threshold, but only leave it
        // once clearly past the wider one. Without this gap, a single 3f step barely
        // clears the trigger distance, so the next tick immediately walks back toward
        // the player again - flip-flopping every check instead of settling.
        if (isSteppingAside)
        {
            bool clearOfPlayer = distanceToPlayer >= makeWayClearDistance && distanceToPlayerDestination >= makeWayClearDistance;
            if (clearOfPlayer) isSteppingAside = false;
        }
        else if (distanceToPlayer < makeWayTriggerDistance && distanceToPlayerDestination < makeWayTriggerDistance)
        {
            isSteppingAside = true;
            stepIssuedTime = Time.time;
        }

        if (isSteppingAside)
        {
            // Player is crowding us - step out of the way. Issue a fresh retreat step
            // once we've arrived at the last one, OR once we've spent too long stuck
            // (likely blocked by another friendly) without arriving.
            var agent = friendlyCreature.creatureMovement.agent;
            bool arrived = !agent.hasPath || agent.remainingDistance <= agent.stoppingDistance;
            bool stuck = Time.time - stepIssuedTime > stepTimeout;
            if (arrived || stuck)
            {
                Vector3 awayFromPlayer = transform.position - player.transform.position;
                awayFromPlayer.y = 0f;
                if (awayFromPlayer == Vector3.zero) awayFromPlayer = -transform.forward;
                // Randomize the angle a bit each attempt so two friendlies retreating
                // into each other don't just keep pushing along the same line forever.
                awayFromPlayer = Quaternion.Euler(0f, Random.Range(-45f, 45f), 0f) * awayFromPlayer;
                Vector3 backPosition = transform.position + awayFromPlayer.normalized * makeWayBackDistance;

                agent.stoppingDistance = normalStoppingDistance;
                friendlyCreature.creatureMovement.MoveToDestination(backPosition);
                stepIssuedTime = Time.time;
            }
            return;
        }

        if (player.destination == Vector3.zero) friendlyCreature.creatureMovement.MoveToDestination(player.transform.position);
        else friendlyCreature.creatureMovement.MoveToDestination(player.destination);
        friendlyCreature.creatureMovement.agent.stoppingDistance = 2.8f;
    }

    // Occasionally wanders to a random point near wherever guarding started,
    // instead of following the player. Interrupted the moment a target is found.
    private void Guard()
    {
        friendlyCreature.creatureMovement.target = null;

        if (!isGuarding)
        {
            isGuarding = true;
            guardAnchor = transform.position;
        }

        var agent = friendlyCreature.creatureMovement.agent;
        bool arrived = !agent.hasPath || agent.remainingDistance <= agent.stoppingDistance;
        bool stuck = Time.time - guardStepIssuedTime > stepTimeout;
        if ((arrived || stuck) && Random.value < 0.366f) // Chance that it moves
        {
            Vector2 randomOffset = Random.insideUnitCircle * guardRadius;
            Vector3 guardPoint = guardAnchor + new Vector3(randomOffset.x, 0f, randomOffset.y);
            agent.stoppingDistance = normalStoppingDistance;
            friendlyCreature.creatureMovement.MoveToDestination(guardPoint);
            guardStepIssuedTime = Time.time;
        }
    }

    private void UpdateWeaponSelection()
    {
        if (friendlyCreature.onCooldown || weaponSwitchCooldown || !hasRangedWeapon) return;
        if (meleeWeapon == null || rangedWeapon == null) return;
        Transform target = friendlyCreature.creatureMovement.target;
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < rangedWeaponRangeLimit)
        {
            if (!meleeWeapon.gameObject.activeSelf) SwitchToMeleeWeapon();
        }
        else
        {
            if (!rangedWeapon.notAvailable && !rangedWeapon.gameObject.activeSelf) SwitchToRangedWeapon();
        }
    }

    public void SwitchToRangedWeapon()
    {
        friendlyCreature.weaponOnHand = rangedWeapon;
        friendlyCreature.weaponOnHand.canHit = false;
        meleeWeapon.gameObject.SetActive(false);
        rangedWeapon.gameObject.SetActive(true);
        friendlyCreature.SetWeaponBarricadeCollisionHandling();

        friendlyCreature.creatureMovement.animator.SetBool("MeleeEquipped", false);
        friendlyCreature.creatureMovement.animator.SetBool("RangedEquipped", true);

        StartWeaponSwitchCooldown();
    }

    public void SwitchToMeleeWeapon()
    {
        friendlyCreature.weaponOnHand = meleeWeapon;
        friendlyCreature.weaponOnHand.canHit = false;
        rangedWeapon.gameObject.SetActive(false);
        meleeWeapon.gameObject.SetActive(true);
        friendlyCreature.SetWeaponBarricadeCollisionHandling();

        friendlyCreature.creatureMovement.animator.SetBool("RangedEquipped", false);
        friendlyCreature.creatureMovement.animator.SetBool("MeleeEquipped", true);

        StartWeaponSwitchCooldown();
    }

    void StartWeaponSwitchCooldown()
    {
        Invoke("ResetWeaponWheelCooldown", weaponSwitchCoolDownTime);
        weaponSwitchCooldown = true;
    }

    void ResetWeaponWheelCooldown()
    {
        weaponSwitchCooldown = false;
    }
}