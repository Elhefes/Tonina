using UnityEngine;

public class Projectile : Weapon
{
    public Transform shootingPoint;
    public float projectileForce;

    public void SpawnProjectile()
    {
        GameObject projectile = Instantiate(gameObject, shootingPoint.position, shootingPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(projectile.transform.right * projectileForce, ForceMode.Impulse);
        gameObject.SetActive(false);
    }
}
