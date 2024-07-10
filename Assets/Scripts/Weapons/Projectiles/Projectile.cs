using UnityEngine;

public class Projectile : Weapon
{
    public Transform shootingPoint;
    public GameObject directorComponent;

    public void SpawnProjectile()
    {
        // TODO: When projectile cycling is implemented, set canHit to false
        canHit = true;
        directorComponent.SetActive(true);

        GameObject projectile = Instantiate(gameObject, shootingPoint.position, shootingPoint.rotation);
        directorComponent.SetActive(false);
        gameObject.SetActive(false);
    }
}
