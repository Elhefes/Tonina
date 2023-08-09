using UnityEngine;

public class Projectile : Weapon
{
    public Transform shootingPoint;
    public float projectileForce;

    public override void Attack(Animator animator)
    {
        animator.SetTrigger("SpearAttack");

        var quack = new Quaternion(shootingPoint.rotation.w, shootingPoint.rotation.eulerAngles.x, shootingPoint.rotation.eulerAngles.y, shootingPoint.rotation.eulerAngles.z);
        GameObject projectile = Instantiate(gameObject, shootingPoint.position, quack);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileForce, ForceMode.Impulse);
        print("niin");

        gameObject.SetActive(false);
    }
}
