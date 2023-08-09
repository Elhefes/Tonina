using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public int health;

    public Weapon weaponOnHand;
    public GameObject[] weaponObjects;
    private Coroutine attackCoroutine;

    public PlayerMovement playerMovement;

    public float attackDistance;

    private float attackAngleThreshold = 10f;

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 114f, Color.red);
        if (playerMovement.target)
        {
            playerMovement.agent.destination = playerMovement.target.position;

            var directionToTarget = playerMovement.target.position - transform.position;
            directionToTarget.y = 0;
            var angle = Vector3.Angle(transform.forward, directionToTarget);
            Debug.DrawLine(transform.position, transform.position + directionToTarget * 114f, Color.red);
            print(angle);
            if (Vector3.Distance(transform.position, playerMovement.target.position) <= attackDistance && angle < attackAngleThreshold)
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
                playerMovement.agent.SetDestination(hit.point);
            }
        }
    }

    IEnumerator Attack()
    {
        while (playerMovement.enemy)
        {
            weaponOnHand.Attack(playerMovement.playerAnimator);
            yield return new WaitForSeconds(1);
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
    }
}
