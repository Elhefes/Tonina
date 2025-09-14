using UnityEngine;
using TMPro;

public class Projectile : Weapon
{
    public Transform shootingPoint;
    public GameObject directorComponent;
    public int quantity;
    public bool infinite;
    public TMP_Text weaponWheelQuantityTMP;

    protected bool projectileInPool = true;

    public void SpawnProjectile()
    {
        canHit = true;
        directorComponent.SetActive(true);

        if (!infinite) quantity--;
        if (quantity < 1) notAvailable = true;
        if (weaponWheelQuantityTMP != null) weaponWheelQuantityTMP.text = quantity.ToString();

        GameObject projectile = Instantiate(gameObject, shootingPoint.position, shootingPoint.rotation);
        canHit = false;
        directorComponent.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetProjectileInPool(bool value)
    {
        projectileInPool = value;
    }

    public void Destroy()
    {
        CancelInvoke();
        if (!gameObject.activeSelf) return;
        //if (projectileInPool) ObjectPooler.Instance.AddProjectileToPool(this, gameObject.name.Remove(gameObject.name.Length - 7));
        //else Destroy(gameObject);
        Destroy(gameObject);
    }
}
