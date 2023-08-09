using UnityEngine;

public class Projectile : Weapon
{
    public Transform shootingPoint;
    public float projectileForce;

    public override void Attack(Animator animator)
    {
        animator.SetTrigger("SpearAttack");

        GameObject projectile = Instantiate(gameObject, shootingPoint.position, shootingPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileForce, ForceMode.Impulse);
        print("niin");

        gameObject.SetActive(false);
    }
}
