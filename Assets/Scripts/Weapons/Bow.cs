using UnityEngine;

public class Bow : Projectile
{
    public Arrow arrow;
    public Rigidbody arrowRb;

    public override void Attack(Animator animator)
    {
        //animator.SetTrigger("BowAttack");
        SpawnArrow();
    }

    void SpawnArrow()
    {
        arrow.directorComponent.SetActive(true);
        arrow.canHit = true;

        quantity--;
        if (quantity < 1) notAvailable = true;
        if (weaponWheelQuantityTMP != null) weaponWheelQuantityTMP.text = quantity.ToString();

        arrowRb.constraints = RigidbodyConstraints.None;

        Instantiate(arrow.gameObject, shootingPoint.position, shootingPoint.rotation);
        arrowRb.constraints = RigidbodyConstraints.FreezeAll;
        arrow.directorComponent.SetActive(false);
        arrow.canHit = false;
        // Need to find a way to disable and enable arrow after shooting
        //arrow.gameObject.SetActive(false);
    }
}
