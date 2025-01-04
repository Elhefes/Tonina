using UnityEngine;

public class Bow : Projectile
{
    public Rigidbody arrow;

    public override void Attack(Animator animator)
    {
        //animator.SetTrigger("BowAttack");
        SpawnArrow();
    }

    void SpawnArrow()
    {
        directorComponent.SetActive(true);

        quantity--;
        if (quantity < 1) notAvailable = true;
        if (weaponWheelQuantityTMP != null) weaponWheelQuantityTMP.text = quantity.ToString();

        arrow.constraints = RigidbodyConstraints.None;

        Instantiate(arrow, shootingPoint.position, shootingPoint.rotation);
        arrow.constraints = RigidbodyConstraints.FreezeAll;
        directorComponent.SetActive(false);
        // Need to find a way to disable and enable arrow after shooting
        //arrow.gameObject.SetActive(false);

        // Also make it so that arrows hit only 1 enemy
    }
}
