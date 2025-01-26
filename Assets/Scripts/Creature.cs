using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Weapon weaponOnHand;
    public CreatureMovement creatureMovement;
    private Coroutine attackCoroutine;
    public bool onCooldown;
    public bool shouldAttack;

    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        if (creatureMovement.target)
        {
            creatureMovement.agent.destination = creatureMovement.target.position;

            var directionToTarget = creatureMovement.target.position - transform.position;
            directionToTarget.y = 0;
            var angle = Vector3.Angle(transform.forward, directionToTarget);
            Debug.DrawLine(transform.position, transform.position + directionToTarget * 114f, Color.red);

            // This could be optimized in theory by replacing shouldAttack with 'float distanceToTarget', but it didn't work in testing
            if (creatureMovement.agent.velocity != Vector3.zero)
            {
                shouldAttack = weaponOnHand.ShouldAttack(Vector3.Distance(transform.position, creatureMovement.target.position), angle);
            }
            else
            {
                RotateTowardsTarget();
                // 0.5f is for attacking Fences, needs changes when Tower is created
                shouldAttack = weaponOnHand.ShouldAttack(Vector3.Distance(transform.position, creatureMovement.target.position) - 0.5f, 0f);
            }

            if (shouldAttack)
            {
                if (!onCooldown) attackCoroutine = StartCoroutine(Attack());
                return;
            }
            else if (Vector3.Distance(transform.position, creatureMovement.target.position) < weaponOnHand.attackDistance - 0.5f)
            {
                LookAt(creatureMovement.target, false);
                creatureMovement.agent.isStopped = true;
                return;
            }
        }
        creatureMovement.agent.isStopped = false;
    }

    public void LookAt(Transform objToLookAt, bool stopInFront)
    {
        if ((!stopInFront && Vector3.Distance(transform.position, objToLookAt.position) <= weaponOnHand.attackDistance || 
            (Vector3.Distance(transform.position, objToLookAt.position) <= creatureMovement.agent.stoppingDistance + 1f)))
        {
            RotateTowardsTarget();
        }
    }

    private void RotateTowardsTarget()
    {
        // Calculate the direction to the destination
        Vector3 direction = (creatureMovement.agent.destination - transform.position).normalized;

        // Ignore the y-axis to prevent tilting
        direction.y = 0f;

        // Rotate towards the destination
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * creatureMovement.agent.angularSpeed * 0.25f);
        }
    }

    protected virtual IEnumerator Attack()
    {
        onCooldown = true;
        while (creatureMovement.target && shouldAttack && !weaponOnHand.notAvailable)
        {
            weaponOnHand.Attack(creatureMovement.animator);
            yield return new WaitForSeconds(weaponOnHand.attackCooldown);

            // Projectile respawn on hand
            if (!weaponOnHand.gameObject.activeSelf && !weaponOnHand.notAvailable) weaponOnHand.gameObject.SetActive(true);
        }
        onCooldown = false;
    }
}
