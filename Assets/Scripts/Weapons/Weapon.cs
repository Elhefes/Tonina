using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public WeaponType type;
    public int damage;
    public Sprite uiSprite;

    public virtual void Attack(Animator animator)
    {

    }

    void Update()
    {
        /*
        if (playerMovement.target)
        {
            playerMovement.agent.destination = playerMovement.target.position;
            if (Vector3.Distance(transform.position, playerMovement.target.position) <= attackDistance)
            {
                if (attackCoroutine == null) attackCoroutine = StartCoroutine(Attack());
            }
            else
            {
                if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
        */
    }
}

public enum WeaponType
{
    Club,
    Small_stone,
    Spear
}