using UnityEngine;

public class Projectile : Weapon
{
    public Transform shootingPoint;
    public float projectileForce;

    public void SpawnProjectile()
    {
        // TODO: When projectile cycling is implemented, set canHit to false
        canHit = true;

        GameObject projectile = Instantiate(gameObject, shootingPoint.position, shootingPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(projectile.transform.right * projectileForce, ForceMode.Impulse);
        gameObject.SetActive(false);
    }
}
