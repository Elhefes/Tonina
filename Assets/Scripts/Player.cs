using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int health;
    public int startingHealth;
    public Weapon weaponOnHand;
    public GameObject[] weaponObjects;
    private Coroutine attackCoroutine;
    public PlayerMovement playerMovement;
    public Slider healthBar;

    public PlayerHealthIndicator playerHealthIndicator;

    public Light weaponRangeIndicatorLight;

    private void Awake()
    {
        health = startingHealth;
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 114f, Color.red);

        if (playerMovement.target)
        {
            playerMovement.agent.destination = playerMovement.target.position;

            var directionToTarget = playerMovement.target.position - transform.position;
            directionToTarget.y = 0;
            var angle = Vector3.Angle(transform.forward, directionToTarget);
            Debug.DrawLine(transform.position, transform.position + directionToTarget * 114f, Color.red);
            //print(angle);

            // Move the attack condition logic to the Weapon class
            bool shouldAttack = weaponOnHand.ShouldAttack(Vector3.Distance(transform.position, playerMovement.target.position), angle);

            if (shouldAttack)
            {
                if (attackCoroutine == null) attackCoroutine = StartCoroutine(Attack());
            }
            else
            {
                if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            // Check if there is a touch
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Check if finger is over a UI element
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }
            }
            if (attackCoroutine != null) StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            playerMovement.target = null;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                GameObject target = hit.collider.gameObject;
                if (target.CompareTag("Enemy"))
                {
                    playerMovement.agent.stoppingDistance = 1.5f;
                    playerMovement.enemy = target.GetComponent<EnemyAI>();
                    playerMovement.target = target.transform;
                }
                else playerMovement.agent.stoppingDistance = 0.1f;
                playerMovement.agent.SetDestination(hit.point);
            }
        }

        if (weaponRangeIndicatorLight.intensity > 0)
        {
            weaponRangeIndicatorLight.intensity -= 0.007f;
        }
    }

    private IEnumerator Attack()
    {
        while (playerMovement.enemy)
        {
            weaponOnHand.Attack(playerMovement.playerAnimator);
            yield return new WaitForSeconds(weaponOnHand.attackCooldown);
        }
    }

    public void SwitchWeapon(WeaponType weaponType)
    {
        foreach (GameObject obj in weaponObjects)
        {
            obj.SetActive(false);
        }

        int weaponIndex = 0;
        if (weaponType == WeaponType.Small_stone) weaponIndex = 1;
        if (weaponType == WeaponType.Spear) weaponIndex = 2;

        weaponOnHand = weaponObjects[weaponIndex].GetComponent<Weapon>();
        weaponObjects[weaponIndex].SetActive(true);
        playerMovement.playerAnimator.SetInteger("WeaponIndex", weaponIndex);
        UpdateWeaponRangeIndicator();
    }

    private void UpdateWeaponRangeIndicator()
    {
        weaponRangeIndicatorLight.spotAngle = Mathf.Asin((weaponOnHand.attackDistance / 25f)) * 180 / Mathf.PI;
        weaponRangeIndicatorLight.intensity = 1.75f;
    }

    public void TakeDamage(int damage)
    {
        healthBar.value -= (float)damage / startingHealth;
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        playerHealthIndicator.UpdateHealthIndicator(health);
        print(health);
    }
}