using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Weapon weaponOnHand;
    public CreatureMovement creatureMovement;
    private Coroutine attackCoroutine;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (creatureMovement.target)
        {
            creatureMovement.agent.destination = creatureMovement.target.position;
            //print(playerMovement.target.position);
            //print("niin");

            var directionToTarget = creatureMovement.target.position - transform.position;
            directionToTarget.y = 0;
            var angle = Vector3.Angle(transform.forward, directionToTarget);
            Debug.DrawLine(transform.position, transform.position + directionToTarget * 114f, Color.red);
            bool shouldAttack = weaponOnHand.ShouldAttack(Vector3.Distance(transform.position, creatureMovement.target.position), angle);

            if (shouldAttack)
            {
                if (attackCoroutine == null) attackCoroutine = StartCoroutine(Attack());
            }
            else
            {
                if (Vector3.Distance(transform.position, creatureMovement.target.position) <= creatureMovement.agent.stoppingDistance + 1f)
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
                if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
        // Move the attack condition logic to the Weapon class
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        attackCoroutine = null;
    }
    private IEnumerator Attack()
    {
        while (creatureMovement.enemy)
        {
            weaponOnHand.Attack(creatureMovement.animator);
            yield return new WaitForSeconds(weaponOnHand.attackCooldown);
        }
    }
}
